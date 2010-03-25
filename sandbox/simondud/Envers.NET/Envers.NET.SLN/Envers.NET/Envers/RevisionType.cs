/* TODO Simon - modifica textul de copyright
 * Hibernate, Relational Persistence for Idiomatic Java
 *
 * Copyright (c) 2008, Red Hat Middleware LLC or third-party contributors as
 * indicated by the @author tags or express copyright attribution
 * statements applied by the authors.  All third-party contributions are
 * distributed under license by Red Hat Middleware LLC.
 *
 * This copyrighted material is made available to anyone wishing to use, modify,
 * copy, or redistribute it subject to the terms and conditions of the GNU
 * Lesser General Public License, as published by the Free Software Foundation.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
 * or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU Lesser General Public License
 * for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this distribution; if not, write to:
 * Free Software Foundation, Inc.
 * 51 Franklin Street, Fifth Floor
 * Boston, MA  02110-1301  USA
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public static readonly RevisionType ADD = new RevisionType((byte) 0);

        /**
         * Indicates that the entity was modified (one or more of its fields) at that revision.
         */
        public static readonly RevisionType MOD = new RevisionType((byte) 1);
        /**
         * Indicates that the entity was deleted (removed) at that revision.
         */
        public static readonly RevisionType DEL = new RevisionType((byte) 2);

        public Byte Representation {get; set;}

        public RevisionType(byte representation)
        {
            Representation = representation;
        }

        public static RevisionType fromRepresentation(Object representation) {
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
