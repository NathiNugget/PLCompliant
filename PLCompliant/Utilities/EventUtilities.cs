using PLCompliant.EventArguments;

namespace PLCompliant.Utilities
{
    /// <summary>
    /// Static class for validation of Context and Arguments for events. 
    /// </summary>
    public static class EventUtilities
    {
        /// <summary>
        /// For some reason, it cannot be fully generic as typically seen in C# methods
        /// </summary>
        /// <typeparam name="C">The expected output Context type</typeparam>
        /// <typeparam name="A">The expected output Argyment type</typeparam>
        /// <typeparam name="CFROM">The input Context type</typeparam>
        /// <typeparam name="AFROM">The inupt Argument type</typeparam>
        /// <param name="context">The input context</param>
        /// <param name="argument">The input argument</param>
        /// <returns>Tuple containing the expected types</returns>
        /// <exception cref="ArgumentNullException">Thrown if a argument is null</exception>
        /// <exception cref="InvalidCastException">Thrown if a argument could not be casted to the specified type</exception>
        public static Tuple<C, A> ValidateContextAndArgs<C, A, CFROM, AFROM>(CFROM context, AFROM argument)
            where CFROM : class
            where C : CFROM
            where A : RaisedEventArgs
        {
            if (context == null)
            {
                throw new ArgumentNullException("");
            }
            if (argument == null)
            {
                throw new ArgumentNullException(nameof(argument));
            }

            C? form = (C)context;
            if (form == null)
            {
                throw new InvalidCastException($"Forkerte runtime type: {argument.GetType()}");
            }

            A? args = argument as A;
            if (args == null)
            {
                throw new InvalidCastException(nameof(args));
            }
            return new Tuple<C, A>(form, args);
        }


        /// <summary>
        /// Validation of a Context
        /// </summary>
        /// <typeparam name="C">The expected output Context type</typeparam>
        /// <typeparam name="CFROM">The input Context type</typeparam>
        /// <param name="context">Input context</param>
        /// <returns>Validated context</returns>
        /// <exception cref="ArgumentNullException">Thrown if context is null</exception>
        /// <exception cref="InvalidCastException">Thrown if context could not be casted to the expected output type</exception>
        public static C ValidateContext<C, CFROM>(CFROM context)
            where CFROM : class
            where C : CFROM
        {
            if (context == null)
            {
                throw new ArgumentNullException();
            }


            C? ctx = (C)context;
            if (ctx == null)
            {
                throw new InvalidCastException(nameof(context));
            }


            return ctx;
        }
    }
}
