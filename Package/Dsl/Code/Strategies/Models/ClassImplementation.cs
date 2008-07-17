using DSLFactory.Candle.SystemModel.CodeGeneration;
using DSLFactory.Candle.SystemModel.Strategies;

namespace DSLFactory.Candle.SystemModel
{
    partial class ClassImplementation
    {
        /// <summary>
        /// Gets the strategies owner.
        /// </summary>
        /// <value>The strategies owner.</value>
        public override CandleElement StrategiesOwner
        {
            get { return Layer.StrategiesOwner; }
        }

        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public override ICustomizableElement Owner
        {
            get { return Layer; }
        }

        /// <summary>
        /// Generates the code.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        internal override bool GenerateCode(GenerationContext context)
        {
            if (context.CanGenerate(Id))
            {
                Generator.ApplyStrategies(this, context);
                if (context.IsModelSelected(Id))
                    return true;
            }
            return false;
        }
    }
}