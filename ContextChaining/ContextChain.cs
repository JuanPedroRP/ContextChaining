using System;

namespace ContextChaining
{
    public sealed class ContextChain
    {
        private Type _memberType;
        private object _member;

        public ContextChain()
        {
        }

        private ContextChain(ContextChain parentContext)
        {
            Parent = parentContext;
        }
        
        public ContextChain Parent { get; }

        public ContextChain Append<T>(T member)
            where T : class
        {
            return new ContextChain(this)
            {
                _member = member ?? throw new ArgumentNullException(nameof(member)),
                _memberType = typeof(T)
            };
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Common use.")]
        public ContextChain Hide<T>()
            where T : class
        {
            return new ContextChain(this)
            {
                _memberType = typeof(T),
            };
        }

        public T Find<T>()
            where T : class
        {
            var current = this;
            var type = typeof(T);

            while (current != null)
            {
                if (current._memberType == type)
                {
                    return (T)current._member;
                }

                current = current.Parent;
            }

            return null;
        }

        public bool Containts(ContextChain contextChain)
        {
            var current = this;
            while (current != null)
            {
                if (current == contextChain)
                {
                    return true;
                }
                current = contextChain.Parent;
            }

            return false;
        }
    }
}
