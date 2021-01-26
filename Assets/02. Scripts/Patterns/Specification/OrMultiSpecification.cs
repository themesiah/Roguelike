namespace Laresistance.Patterns
{
    public class OrMultiSpecification<T> : CompositeSpecification<T>
    {
        ISpecification<T>[] specifications;

        public OrMultiSpecification(ISpecification<T>[] specifications)
        {
            this.specifications = specifications;
        }

        public override bool IsSatisfiedBy(T candidate)
        {
            foreach(ISpecification<T> specification in specifications)
            {
                if (specification.IsSatisfiedBy(candidate))
                {
                    return true;
                }
            }
            return false;
        }
    }
}