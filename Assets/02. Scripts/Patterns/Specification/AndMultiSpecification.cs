namespace Laresistance.Patterns
{
    public class AndMultiSpecification<T> : CompositeSpecification<T>
    {
        ISpecification<T>[] specifications;

        public AndMultiSpecification(ISpecification<T>[] specifications)
        {
            this.specifications = specifications;
        }

        public override bool IsSatisfiedBy(T candidate)
        {
            foreach(var specification in specifications)
            {
                if (!specification.IsSatisfiedBy(candidate))
                {
                    return false;
                }
            }
            return true;
        }
    }
}