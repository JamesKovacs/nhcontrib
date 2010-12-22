using System;

namespace NHibernate.Envers
{

    /**
     * Type of the revision.
     * @author Simon Duduica, port of Envers omonyme class by Adam Warski (adam at warski dot org)
     */
    public class RevisionType {
        /**
         * Indicates that the entity was added (persisted) at that revision.
         */
        public static readonly RevisionType ADD = new RevisionType(0);

        /**
         * Indicates that the entity was modified (one or more of its fields) at that revision.
         */
        public static readonly RevisionType MOD = new RevisionType(1);
        /**
         * Indicates that the entity was deleted (removed) at that revision.
         */
        public static readonly RevisionType DEL = new RevisionType(2);

        public Byte Representation {get; private set;}

    	private RevisionType(byte representation)
        {
            Representation = representation;
        }

        public static RevisionType FromRepresentation(object representation) {
            if (representation == null || !(representation is Byte)) {
                return null;
            }

            switch ((Byte) representation) {
                case 0: return ADD;
                case 1: return MOD;
                case 2: return DEL;
            }

            throw new ArgumentOutOfRangeException("Unknown representation: " + representation);
        }
    }
}
