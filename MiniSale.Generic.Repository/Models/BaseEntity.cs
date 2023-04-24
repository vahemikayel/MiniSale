using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSale.Generic.Repository.Models
{
    public class BaseEntity<TIdentity> where TIdentity : IComparable, IComparable<TIdentity>, IEquatable<TIdentity>
    {
        private int? _requestedHashCode;

        public virtual TIdentity Id { get; set; }

        public bool IsTransient()
        {
            return EqualityComparer<TIdentity>.Default.Equals(Id, default(TIdentity));
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is BaseEntity<TIdentity>))
            {
                return false;
            }

            if (this == obj)
            {
                return true;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            BaseEntity<TIdentity> baseEntity = (BaseEntity<TIdentity>)obj;
            if (baseEntity.IsTransient() || IsTransient())
            {
                return false;
            }

            return EqualityComparer<TIdentity>.Default.Equals(baseEntity.Id, Id);
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                {
                    _requestedHashCode = Id.GetHashCode() ^ 0x1F;
                }

                return _requestedHashCode.Value;
            }

            return base.GetHashCode();
        }

        public static bool operator ==(BaseEntity<TIdentity> left, BaseEntity<TIdentity> right)
        {
            if (object.Equals(left, null))
            {
                if (!object.Equals(right, null))
                {
                    return false;
                }

                return true;
            }

            return left.Equals(right);
        }

        public static bool operator !=(BaseEntity<TIdentity> left, BaseEntity<TIdentity> right)
        {
            return !(left == right);
        }
    }
}
