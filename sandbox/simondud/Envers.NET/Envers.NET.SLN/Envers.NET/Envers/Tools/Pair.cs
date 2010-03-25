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
        private T1 obj1;
        private T2 obj2;

        public Pair(T1 obj1, T2 obj2) {
            this.obj1 = obj1;
            this.obj2 = obj2;
        }

        public T1 getFirst() {
            return obj1;
        }

        public T2 getSecond() {
            return obj2;
        }

        public bool equals(Object o) {
            if (this == o) return true;
            if (!(o.GetType().Equals(typeof(Pair<,>)))) return false;

            Pair<object, object> pair = (Pair<object,object>) o;

            if (obj1 != null ? !obj1.Equals(pair.obj1) : pair.obj1 != null) return false;
            if (obj2 != null ? !obj2.Equals(pair.obj2) : pair.obj2 != null) return false;

            return true;
        }

        public int hashCode() {
            int result;
            result = (obj1 != null ? obj1.GetHashCode() : 0);
            result = 31 * result + (obj2 != null ? obj2.GetHashCode() : 0);
            return result;
        }

        public static Pair<T1, T2> Make<T1, T2>(T1 obj1, T2 obj2)
        {
            return new Pair<T1, T2>(obj1, obj2);
        }
    }
}
