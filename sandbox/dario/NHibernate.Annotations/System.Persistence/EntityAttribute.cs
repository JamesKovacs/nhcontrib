namespace System.Persistence
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityAttribute : Attribute
    {
		//TODO: doc
		public bool IsMutable { get; set; }
		public bool IsDynamicInsert { get; set; }
		public bool IsDynamicUpdate { get; set; }
		public bool IsSelectBeforeUpdate { get; set; }
		public PolymorphismType Polymorphism { get; set; }
		public string Persister { get; set; }
		public OptimisticLockType OptimisticLock { get; set; }
    }

//@Target(TYPE)
//@Retention(RUNTIME)
//public @interface EntityAttribute {
//    /** Is this entity mutable (read only) or not */
//    boolean mutable() default true;
//    /** Needed column only in SQL on insert */
//    boolean dynamicInsert() default false;
//    /** Needed column only in SQL on update */
//    boolean dynamicUpdate() default false;
//    /** Do a select to retrieve the entity before any potential update */
//    boolean selectBeforeUpdate() default false;
//    /** polymorphism strategy for this entity */
//    PolymorphismType polymorphism() default PolymorphismType.IMPLICIT;
//    /** persister of this entity, default is hibernate internal one */
//    String persister() default "";
//    /** optimistic locking strategy */
//    OptimisticLockType optimisticLock() default OptimisticLockType.VERSION;
//}
}