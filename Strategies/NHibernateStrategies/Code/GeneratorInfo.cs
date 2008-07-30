using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;

namespace DSLFactory.Candle.SystemModel.Strategies.NHibernate
{
    /// <summary>
    /// Information sur le generateur de séquence nhibernate
    /// </summary>
    [TypeConverter(typeof(GeneratorInfoTypeConverter))]
    public class GeneratorInfo
    {
        public GeneratorInfo()
        {
        }

        public GeneratorInfo(string name)
        {
            this.Name = name;
        }

        public class GeneratorParm
        {
            public string Name;
            public string Value;
        }

        public string Name;
        public List<GeneratorParm> Parms = new List<GeneratorParm>();
    }


    public class GeneratorInfoTypeConverter : TypeConverter
    {
        public override bool CanConvertTo( ITypeDescriptorContext context, Type destinationType )
        {
            if( destinationType == typeof( string ) )
                return true;
            return base.CanConvertTo( context, destinationType );
        }

        public override bool CanConvertFrom( ITypeDescriptorContext context, Type sourceType )
        {
            if( sourceType == typeof( string ) )
                return true;
            return base.CanConvertFrom( context, sourceType );
        }

        public override object ConvertFrom( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value )
        {
            if( value is string && value != null)
            {
                string[] items = ( (string)value ).Split( ';');
                GeneratorInfo gi = new GeneratorInfo();
                if( items.Length > 0 )
                {
                    gi.Name = items[0];

                    for( int i=1; i<items.Length; i++ )
                    {
                        string txt = items[i];
                        string[] parts = txt.Split( '=' );
                        GeneratorInfo.GeneratorParm parm = new GeneratorInfo.GeneratorParm();
                        parm.Name = parts[0].Trim();
                        parm.Value = parts[1].Trim();
                        gi.Parms.Add( parm );
                    }
                }
                return gi;
            }

            return base.ConvertFrom( context, culture, value );
        }

        public override object ConvertTo( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType )
        {
            if(value != null && destinationType == typeof( string ) )
            {
                GeneratorInfo gi = value as GeneratorInfo;
                StringBuilder sb = new StringBuilder( gi.Name );
                foreach( GeneratorInfo.GeneratorParm parm in gi.Parms )
                {
                    if (!String.IsNullOrEmpty(parm.Name))
                    {
                        sb.Append(';');
                        sb.Append(parm.Name);
                        sb.Append('=');
                        sb.Append(parm.Value);
                    }
                }
                return sb.ToString();
            }

            return base.ConvertTo( context, culture, value, destinationType );
        }
    }

    public class GeneratorInfoTypeEditor : UITypeEditor
    {
        public override object EditValue( System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value )
        {
            GeneratorInfo gi = value as GeneratorInfo;
            if( gi == null )
                gi = new GeneratorInfo();

            GeneratorInfoEditorDialog dlg = new GeneratorInfoEditorDialog( gi );
            if( dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK )
            {
                return dlg.Value;
            }
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle( System.ComponentModel.ITypeDescriptorContext context )
        {
            return UITypeEditorEditStyle.Modal;
        }        
    }
}
