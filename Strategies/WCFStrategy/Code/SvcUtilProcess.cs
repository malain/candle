using System;
using System.Text;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Collections;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Classe encapsulant l'appel de svcutil permettant de générer le proxy
    /// </summary>
    internal class SvcUtilProcess
    {
        private ManualResetEvent processSemaphore;
        private ManualResetEvent outputEvent;
        private ManualResetEvent timeoutSemaphore;
        private Queue outStream;
        private string errorMessage;

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>The error message.</value>
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }

        /// <summary>
        /// Creates the proxy.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="newNamespace">The new namespace.</param>
        /// <param name="mexAddress">The mex address.</param>
        /// <returns></returns>
        internal int CreateProxy(string folder, string newNamespace, string mexAddress)
        {
            string toolCmdLine = string.Format("/noLogo /d:\"{0}\" /config:\"{0}\\{1}\" /mergeConfig /out:\"{0}\\{2}\" /language:{3} /n:*,{4} {5}",
                folder, "app.config", "proxy", "C#", newNamespace, mexAddress);
            return RunInternal(folder, toolCmdLine);
        }

        /// <summary>
        /// Creates the WSDL.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <returns></returns>
        internal int CreateWsdl(string folder, string assemblyPath)
        {
            return RunInternal(folder, assemblyPath);
        }

        /// <summary>
        /// Execute svcutil
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="cmdLineArgs">The CMD line args.</param>
        /// <returns></returns>
        private int RunInternal(string folder, string cmdLineArgs)
        {
            ErrorMessage = String.Empty;

            string exe = GetSvcUtilPath();
            if (exe == null)
            {
                throw new Exception("svcutil not found.");
            }

            ProcessStartInfo info = new ProcessStartInfo(exe, cmdLineArgs);
            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.RedirectStandardInput = true;
            info.WorkingDirectory = folder;

            int exitCode = -1;
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            Timer timer = null;

            try
            {
                this.processSemaphore = new ManualResetEvent(false);
                this.timeoutSemaphore = new ManualResetEvent(false);
                this.outputEvent = new ManualResetEvent(false);
                this.outStream = new Queue();

                proc.StartInfo = info;
                proc.EnableRaisingEvents = true;
                proc.Exited += this.OnProcessExited;
                proc.ErrorDataReceived += this.OnEmitOutput;
//                proc.OutputDataReceived += new DataReceivedEventHandler(this.OnEmitOutput);

                proc.Start();
                proc.StandardInput.Close();
                proc.BeginErrorReadLine();
                proc.BeginOutputReadLine();

                timer = new Timer(this.OnTimeout);
                timer.Change(20000, -1); // 20s

                WaitHandle[] waitHandles = new WaitHandle[] { this.processSemaphore, this.timeoutSemaphore, this.outputEvent };
                bool flag = true;
                while (flag)
                {
                    switch (WaitHandle.WaitAny(waitHandles))
                    {
                        // Terminé
                        case 0:                            
                                EnsureProcessExited(proc);
                                exitCode = proc.ExitCode;
                                flag = false;
                                break;

                        // Timeout atteint
                        case 1:
                            if (!proc.HasExited)
                            {
                                // On arrete proprement le process
                                try {proc.Kill();} catch {} 
                            }
                            else
                                exitCode = proc.ExitCode;

                            EnsureProcessExited(proc);
                            flag = false;
                            break;

                        case 2:
                            PrepareOutput();
                            continue;
                    }
                }
            }
            finally
            {
                if (timer != null)
                {
                    timer.Dispose();
                }

                PrepareOutput();
                
                if (proc != null)
                {
                    try
                    {
                        if (!proc.HasExited)
                        {
                            proc.Kill();
                        }
                        proc.Close();
                    }
                    catch
                    {
                    }
                }
                
                if (this.processSemaphore != null)
                {
                    this.processSemaphore.Close();
                }
                
                if (this.timeoutSemaphore != null)
                {
                    this.timeoutSemaphore.Close();
                }
                
                if (this.outputEvent != null)
                {
                    this.outputEvent.Close();
                }
            }
            return exitCode;
        }


        /// <summary>
        /// Gets the SVC util path.
        /// </summary>
        /// <returns></returns>
        private static string GetSvcUtilPath()
        {
            RegistryKey rk = null;
            try
            {
                rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SDKs\Windows\v6.0");
                string installFolder = rk.GetValue("InstallationFolder") as string;
                return Path.Combine(installFolder, @"bin\svcutil.exe");
            }
            catch
            {
                return null;
            }
            finally
            {
                if( rk!=null)
                    rk.Close();
            }
        }


        /// <summary>
        /// Ensures the process exited.
        /// </summary>
        /// <param name="proc">The proc.</param>
        private static void EnsureProcessExited(System.Diagnostics.Process proc)
        {
            for (int i = 0; (i < 1000) && !proc.HasExited; i++)
            {
                Thread.Sleep(50);
            }
        }

        /// <summary>
        /// Prepares the output.
        /// </summary>
        private void PrepareOutput()
        {
            lock (this.outStream.SyncRoot)
            {
                StringBuilder sb = new StringBuilder(errorMessage);
                while (this.outStream.Count > 0)
                {
                    try
                    {
                     sb.AppendLine(this.outStream.Dequeue() as string);
                    }
                    catch
                    {
                    }
                }
                errorMessage = sb.ToString();
                this.outputEvent.Reset();
            }
        }

        /// <summary>
        /// Called when [timeout].
        /// </summary>
        /// <param name="state">The state.</param>
        private void OnTimeout(object state)
        {
            this.timeoutSemaphore.Set();
        }

        /// <summary>
        /// Called when [process exited].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnProcessExited(object sender, EventArgs e)
        {
            this.processSemaphore.Set();
        }

        /// <summary>
        /// Called when [emit output].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Diagnostics.DataReceivedEventArgs"/> instance containing the event data.</param>
        private void OnEmitOutput(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                lock (this.outStream.SyncRoot)
                {
                    this.outStream.Enqueue(e.Data);
                    this.outputEvent.Set();
                }
            }
        }
    }
}
