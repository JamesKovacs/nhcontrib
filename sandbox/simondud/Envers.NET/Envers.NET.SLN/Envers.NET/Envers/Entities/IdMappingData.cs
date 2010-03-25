using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Envers.Entities.Mapper.Id;
using System.Xml;

namespace NHibernate.Envers.Entities
{
    /**
     * @author Simon Duduica, port of Envers omonyme class by Adam Warski (adam at warski dot org)
     */
    public class IdMappingData {
        public IIdMapper IdMapper { get; private set; }
        // Mapping which will be used to generate the entity
        public XmlElement xmlMapping { get; private set; }
        // Mapping which will be used to generate references to the entity in related entities
        public XmlElement xmlRelationMapping { get; private set; }

        public IdMappingData(IIdMapper idMapper, XmlElement xmlMapping, XmlElement xmlRelationMapping) {
            this.IdMapper = idMapper;
            this.xmlMapping = xmlMapping;
            this.xmlRelationMapping = xmlRelationMapping;
        }
    }
}
