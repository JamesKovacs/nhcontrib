using System.Reflection;
using EnterpriseSample.Core.Domain;
using ProjectBase.Utils;

namespace EnterpriseSample.Tests.Domain
{
    /// <summary>
    /// I've gone back and forth as to when it is appropriate to use reflection for 
    /// accessing/setting a private/protected members.  It is imperitive that the <see cref="DomainObject{IdT}.ID"/>
    /// property is read-only and set only by the ORM.  With that said, some unit tests need 
    /// ID set accordingly; therefore, this utility enables that ability.  This class should 
    /// never be used outside of the unit tests.  Instead, implement <see cref="IHasAssignedId{IdT}" /> to 
    /// expose a public setter.
    /// </summary>
    public class DomainObjectIdSetter<IdT>
    {
        /// <summary>
        /// Uses reflection to set the ID of a <see cref="DomainObject{IdT}" />.
        /// </summary>
        public void SetIdOf(DomainObject<IdT> domainObject, IdT id) {
            // Set the data property reflectively
            PropertyInfo idProperty = domainObject.GetType().GetProperty(NAME_OF_ID_MEMBER,
                BindingFlags.Public | BindingFlags.Instance);

            Check.Ensure(idProperty != null, "idProperty could not be found");

            idProperty.SetValue(domainObject, id, null);
        }

        private const string NAME_OF_ID_MEMBER = "ID";
    }
}
