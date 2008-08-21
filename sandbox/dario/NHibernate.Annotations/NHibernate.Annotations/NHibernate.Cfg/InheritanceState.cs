using System.Persistence;
using NHibernate.Annotations;

namespace NHibernate.Cfg
{
    public class InheritanceState
    {
        private string accessType;
        private bool isPropertyAnnotated;
        public InheritanceType? type;

        public InheritanceState(System.Type clazz)
        {
            Clazz = clazz;
            ExtractInheritanceType();
        }

        public System.Type Clazz { get; set; }

        /// <summary>
        /// has son either mappedsuperclass son or entity son
        /// </summary>
        public bool HasSons { get; set; }

        /// <summary>
        /// a mother entity is available
        /// </summary>
        public bool HasParents { get; set; }

        public bool IsEmbeddableSuperclass { get; set; }

        private void ExtractInheritanceType()
        {
            System.Type element = Clazz;

            InheritanceAttribute inhAnn = AttributeHelper.GetFirst<InheritanceAttribute>(element);
            MappedSuperclassAttribute mappedSuperClass = AttributeHelper.GetFirst<MappedSuperclassAttribute>(element);

            if (mappedSuperClass != null)
            {
                IsEmbeddableSuperclass = true;
                type  = inhAnn == null ? null : inhAnn.Strategy;
            }
            else
            {
                type = inhAnn == null ? InheritanceType.SingleTable  : inhAnn.Strategy;
            }
        }
    }
}