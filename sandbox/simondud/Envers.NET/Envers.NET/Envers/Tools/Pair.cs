using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Envers.Tools
{
    /**
     * A pair of objects.
     * @param <T1>
     * @param <T2>
     * @author Adam Warski (adamw@aster.pl)
     */
    public class Pair<T1, T2> {
        public T1 First{ get; private set;}
        public T2 Second { get; private set; }

        public Pair(T1 obj1, T2 obj2) {
            this.First = obj1;
            this.Second = obj2;
        }

        public override bool Equals(Object o) {
            if (this == o) return true;
            if (!(o.GetType().Equals(typeof(Pair<,>)))) return false;

            Pair<object, object> pair = (Pair<object,object>) o;

            if (First != null ? !First.Equals(pair.First) : pair.First != null) return false;
            if (Second != null ? !Second.Equals(pair.Second) : pair.Second != null) return false;

            return true;
        }

        public override int GetHashCode() {
            int result;
            result = (First != null ? First.GetHashCode() : 0);
            result = 31 * result + (Second != null ? Second.GetHashCode() : 0);
            return result;
        }

        public static Pair<T1, T2> Make<T1, T2>(T1 obj1, T2 obj2)
        {
            return new Pair<T1, T2>(obj1, obj2);
        }
    }
}
