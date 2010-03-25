using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Envers.Entities.Mapper.Id;
using NHibernate.Envers.Entities.Mapper;

namespace NHibernate.Envers.Entities
{
    public class RelationDescription {
        private readonly String fromPropertyName;
        private readonly RelationType relationType;
        private readonly String toEntityName;
        private readonly String mappedByPropertyName;
        private readonly IIdMapper idMapper;
        private readonly IPropertyMapper fakeBidirectionalRelationMapper;
        private readonly IPropertyMapper fakeBidirectionalRelationIndexMapper;
        private readonly bool insertable;
        public bool Bidirectional { get; set; }

        public RelationDescription(String fromPropertyName, RelationType relationType, String toEntityName,
                                   String mappedByPropertyName, IIdMapper idMapper,
                                   IPropertyMapper fakeBidirectionalRelationMapper,
                                   IPropertyMapper fakeBidirectionalRelationIndexMapper, bool insertable) {
            this.fromPropertyName = fromPropertyName;
            this.relationType = relationType;
            this.toEntityName = toEntityName;
            this.mappedByPropertyName = mappedByPropertyName;
            this.idMapper = idMapper;
            this.fakeBidirectionalRelationMapper = fakeBidirectionalRelationMapper;
            this.fakeBidirectionalRelationIndexMapper = fakeBidirectionalRelationIndexMapper;
            this.insertable = insertable;

            this.Bidirectional = false;
        }

        public String getFromPropertyName() {
            return fromPropertyName;
        }

        public RelationType getRelationType() {
            return relationType;
        }

        public String getToEntityName() {
            return toEntityName;
        }

        public String getMappedByPropertyName() {
            return mappedByPropertyName;
        }

        public IIdMapper getIdMapper() {
            return idMapper;
        }

        public IPropertyMapper getFakeBidirectionalRelationMapper() {
            return fakeBidirectionalRelationMapper;
        }

        public IPropertyMapper getFakeBidirectionalRelationIndexMapper() {
            return fakeBidirectionalRelationIndexMapper;
        }

        public bool isInsertable() {
            return insertable;
        }
    }
}
