using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBook.Common.ValueObjects
{
    public abstract class ValueObject : IValueObject
    {
        protected abstract IEnumerable<object> GetAtomicValues();
        public override bool Equals(object toCompare)
        {
            if (toCompare == null || !(toCompare is ValueObject) || GetType() != toCompare.GetType())
                return false;

            if (ReferenceEquals(this, toCompare))
                return true;

            var other = (ValueObject)toCompare;
            IEnumerator<object> thisValues = GetAtomicValues().GetEnumerator();
            IEnumerator<object> otherValues = other.GetAtomicValues().GetEnumerator();

            while (thisValues.MoveNext() && otherValues.MoveNext())
            {
                if (ReferenceEquals(thisValues.Current, null) ^ ReferenceEquals(otherValues.Current, null))
                    return false;

                if (thisValues.Current != null && !thisValues.Current.Equals(otherValues.Current))
                    return false;
            }

            return !thisValues.MoveNext() && !otherValues.MoveNext();
        }

        public override int GetHashCode() => ToString().GetHashCode();

        public static bool operator ==(ValueObject left, ValueObject right)
        {
            if (Equals(left, null))
                return Equals(right, null);
            else
                return left.Equals(right);
        }
        public static bool operator !=(ValueObject left, ValueObject right)
        {
            return !(left == right);
        }
    }
}
