using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Envers.Tools;

namespace NHibernate.Envers.Entities.Mapper
{
    /**
     * Data describing the change of a single object in a persistent collection (when the object was added, removed or
     * modified in the collection).
     * @author Simon Duduica, port of omonyme class by Adam Warski (adam at warski dot org)
     */
    public class PersistentCollectionChangeData {
        public String EntityName
        {
            /**
             * @return Name of the (middle) entity that holds the collection data.
             */
            get;
            private set;
        }
        private readonly IDictionary<String, Object> data;
        private readonly Object changedElement;

        public PersistentCollectionChangeData(String entityName, IDictionary<String, Object> data, Object changedElement) {
            this.EntityName = entityName;
            this.data = data;
            this.changedElement = changedElement;
        }

        public IDictionary<String, Object> getData() {
            return data;
        }

        /**
         * @return The affected element, which was changed (added, removed, modified) in the collection.
         */
        public Object GetChangedElement() {
            if (changedElement.GetType().Equals(typeof(Pair<,>)))
            {
                return ((Pair<Object,Object>)changedElement).Second;
            }

            if (changedElement.GetType().Equals(typeof(KeyValuePair<,>)))
            {
                return ((KeyValuePair<Object,Object>) changedElement).Value;
            }

            return changedElement;
        }

        /**
         * @return Index of the affected element, or {@code null} if the collection isn't indexed.
         */
        public Object GetChangedElementIndex() {
            if (changedElement.GetType().Equals(typeof(Pair<,>)))
            {
                return ((Pair<Object, Object>)changedElement).First;
            }

            if (changedElement.GetType().Equals(typeof(KeyValuePair<,>)))
            {
                return ((KeyValuePair<Object, Object>)changedElement).Key;
            }

            return null;
        }
    }
}
