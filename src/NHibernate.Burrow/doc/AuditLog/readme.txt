1, add the following line into the <MindHarbor.DomainTemplate> section in web.config
    <add key="NHDomain.AuditLog.EnableAuditLog" value="true" />
2, copy the AuditLogRecord.hbm.xml into your DOMAIN/DAL project, make it EmbeddedResources
3, use the AuditLogTable.sql to create a table into your database.
4, if you want to store userId in the auditTable, your need to have a DomainLayer and DomainLayerFactory,
   call YourDomainLayer.Current.CurrentUserId = userId when you obtain the user's identity.