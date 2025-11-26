using PLCompliant.EventArguments;

namespace PLCompliant.Utilities
{
    public static class EventUtilities
    {
        public static Tuple<C, A> ValidateArgs<C, A, CFROM, AFROM>(CFROM context, AFROM argument)
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
