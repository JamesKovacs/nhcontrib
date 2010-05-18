if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_Dispositivos_TMAR_DetallesEstadosTerminal_FK1]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[TMAR_DetallesEstadosTerminal] DROP CONSTRAINT TMAR_Dispositivos_TMAR_DetallesEstadosTerminal_FK1
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_Dispositivos_TMAR_DispositivosTerminal_FK1]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[TMAR_DispositivosTerminal] DROP CONSTRAINT TMAR_Dispositivos_TMAR_DispositivosTerminal_FK1
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_Dispositivos_TMAR_DispositivosTerminales_Hist_FK1]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[TMAR_DispositivosTerminales_Hist] DROP CONSTRAINT TMAR_Dispositivos_TMAR_DispositivosTerminales_Hist_FK1
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_Dispositivos_TMAR_DispositivosTipoTerminal_FK1]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[TMAR_DispositivosTipoTerminal] DROP CONSTRAINT TMAR_Dispositivos_TMAR_DispositivosTipoTerminal_FK1
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_Dispositivos_TMAR_EstadosDispositivos_FK1]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[TMAR_EstadosDispositivos] DROP CONSTRAINT TMAR_Dispositivos_TMAR_EstadosDispositivos_FK1
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_Dispositivos_TMAR_Terminales_FK1]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[TMAR_InfoTerminales] DROP CONSTRAINT TMAR_Dispositivos_TMAR_Terminales_FK1
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_Dispositivos_TMAR_InfoTerminales_Hist_FK1]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[TMAR_InfoTerminales_Hist] DROP CONSTRAINT TMAR_Dispositivos_TMAR_InfoTerminales_Hist_FK1
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_EstadosDispositivos_TMAR_DetallesEstadosTerminal_FK1]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[TMAR_DetallesEstadosTerminal] DROP CONSTRAINT TMAR_EstadosDispositivos_TMAR_DetallesEstadosTerminal_FK1
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_EstadosDispositivos_TMAR_DispositivosTerminal_FK1]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[TMAR_DispositivosTerminal] DROP CONSTRAINT TMAR_EstadosDispositivos_TMAR_DispositivosTerminal_FK1
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_EstadosDispositivos_TMAR_EventosExternos_FK1]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[TMAR_EventosExternos] DROP CONSTRAINT TMAR_EstadosDispositivos_TMAR_EventosExternos_FK1
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_EstadosTerminales_TMAR_DetallesEstadosTerminal_FK1]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[TMAR_DetallesEstadosTerminal] DROP CONSTRAINT TMAR_EstadosTerminales_TMAR_DetallesEstadosTerminal_FK1
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_EstadosTerminales_TMAR_Terminales_FK1]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[TMAR_InfoTerminales] DROP CONSTRAINT TMAR_EstadosTerminales_TMAR_Terminales_FK1
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_EstadosTerminales_TMAR_EstadosTerminales_Hist_FK1]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[TMAR_InfoTerminales_Hist] DROP CONSTRAINT TMAR_EstadosTerminales_TMAR_EstadosTerminales_Hist_FK1
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_Terminales_TMAR_DispositivosTerminal_FK1]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[TMAR_DispositivosTerminal] DROP CONSTRAINT TMAR_Terminales_TMAR_DispositivosTerminal_FK1
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_Terminales_TMAR_DispositivosTerminales_Hist_FK1]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[TMAR_DispositivosTerminales_Hist] DROP CONSTRAINT TMAR_Terminales_TMAR_DispositivosTerminales_Hist_FK1
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_Terminales_TMAR_EstadosTerminales_Hist_FK1]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[TMAR_InfoTerminales_Hist] DROP CONSTRAINT TMAR_Terminales_TMAR_EstadosTerminales_Hist_FK1
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_EstadosTerminales_Hist_TMAR_DispositivosTerminales_Hist_FK1]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[TMAR_DispositivosTerminales_Hist] DROP CONSTRAINT TMAR_EstadosTerminales_Hist_TMAR_DispositivosTerminales_Hist_FK1
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_Procesos_TMAR_EventosExternos_FK1]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[TMAR_EventosExternos] DROP CONSTRAINT TMAR_Procesos_TMAR_EventosExternos_FK1
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_Procesos_TMAR_TipoTerminales_FK1]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[TMAR_TipoTerminales] DROP CONSTRAINT TMAR_Procesos_TMAR_TipoTerminales_FK1
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_Severidad_TMAR_EstadosDispositivos_FK1]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[TMAR_EstadosDispositivos] DROP CONSTRAINT TMAR_Severidad_TMAR_EstadosDispositivos_FK1
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_Severidad_TMAR_EstadosTerminales_FK1]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[TMAR_EstadosTerminales] DROP CONSTRAINT TMAR_Severidad_TMAR_EstadosTerminales_FK1
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_TipoTerminales_TMAR_DispositivosTipoTerminal_FK1]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[TMAR_DispositivosTipoTerminal] DROP CONSTRAINT TMAR_TipoTerminales_TMAR_DispositivosTipoTerminal_FK1
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_TipoTerminales_TMAR_EstadosTerminales_FK1]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[TMAR_EstadosTerminales] DROP CONSTRAINT TMAR_TipoTerminales_TMAR_EstadosTerminales_FK1
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_TipoTerminales_TMAR_Terminales_FK1]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[TMAR_InfoTerminales] DROP CONSTRAINT TMAR_TipoTerminales_TMAR_Terminales_FK1
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_DetallesEstadosTerminal]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TMAR_DetallesEstadosTerminal]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_Dispositivos]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TMAR_Dispositivos]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_DispositivosTerminal]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TMAR_DispositivosTerminal]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_DispositivosTerminales_Hist]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TMAR_DispositivosTerminales_Hist]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_DispositivosTipoTerminal]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TMAR_DispositivosTipoTerminal]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_EstadosDispositivos]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TMAR_EstadosDispositivos]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_EstadosTerminales]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TMAR_EstadosTerminales]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_EventosExternos]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TMAR_EventosExternos]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_InfoTerminales]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TMAR_InfoTerminales]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_InfoTerminales_Hist]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TMAR_InfoTerminales_Hist]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_Procesos]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TMAR_Procesos]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_Severidad]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TMAR_Severidad]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TMAR_TipoTerminales]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TMAR_TipoTerminales]
GO

CREATE TABLE [dbo].[TMAR_DetallesEstadosTerminal] (
	[ID_EstadoTerminal] [smallint] NOT NULL ,
	[NroTermino] [smallint] NOT NULL ,
	[ParentesisIzq] [bit] NOT NULL ,
	[ID_Dispositivo] [smallint] NOT NULL ,
	[Operador] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ID_Estado] [smallint] NOT NULL ,
	[ParentesisDer] [bit] NOT NULL ,
	[Relacion] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ID_TipoTerminal] [smallint] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[TMAR_Dispositivos] (
	[ID_Dispositivo] [smallint] NOT NULL ,
	[Nombre] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Virtual] [bit] NOT NULL ,
	[Procedimiento] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[TMAR_DispositivosTerminal] (
	[ID_Terminal] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ID_Dispositivo] [smallint] NOT NULL ,
	[ID_Estado] [smallint] NOT NULL ,
	[FechaEstado] [datetime] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[TMAR_DispositivosTerminales_Hist] (
	[ID_Terminal] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ID_Dispositivo] [smallint] NOT NULL ,
	[FechaInicio] [datetime] NOT NULL ,
	[ID_Estado] [smallint] NOT NULL ,
	[FechaFin] [datetime] NOT NULL ,
	[TiempoTotal] [bigint] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[TMAR_DispositivosTipoTerminal] (
	[ID_TipoTerminal] [smallint] NOT NULL ,
	[ID_Dispositivo] [smallint] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[TMAR_EstadosDispositivos] (
	[ID_Dispositivo] [smallint] NOT NULL ,
	[ID_Estado] [smallint] NOT NULL ,
	[Descripcion] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ID_Severidad] [smallint] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[TMAR_EstadosTerminales] (
	[ID_EstadoTerminal] [smallint] NOT NULL ,
	[EstadoTerminal] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[OrdenEvaluacion] [smallint] NOT NULL ,
	[ID_Severidad] [smallint] NOT NULL ,
	[ID_TipoTerminal] [smallint] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[TMAR_EventosExternos] (
	[ID_Proceso] [smallint] NOT NULL ,
	[Mensaje] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ID_Dispositivo] [smallint] NOT NULL ,
	[ID_Estado] [smallint] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[TMAR_InfoTerminales] (
	[ID_Terminal] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ID_EstadoTerminal] [smallint] NOT NULL ,
	[ID_TipoTerminal] [smallint] NOT NULL ,
	[FechaEstado] [datetime] NOT NULL ,
	[ID_DispositivoCausa] [smallint] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[TMAR_InfoTerminales_Hist] (
	[ID_Terminal] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[FechaInicio] [datetime] NOT NULL ,
	[ID_EstadoTerminal] [smallint] NOT NULL ,
	[FechaFin] [datetime] NOT NULL ,
	[TiempoTotal] [bigint] NOT NULL ,
	[ID_DispositivoCausa] [smallint] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[TMAR_Procesos] (
	[ID_Proceso] [smallint] NOT NULL ,
	[Proceso] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[TMAR_Severidad] (
	[ID_Severidad] [smallint] NOT NULL ,
	[Severidad] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[TMAR_TipoTerminales] (
	[ID_TipoTerminal] [smallint] NOT NULL ,
	[Descripcion] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ID_Proceso] [smallint] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TMAR_DetallesEstadosTerminal] WITH NOCHECK ADD 
	CONSTRAINT [PK_TMAR_DetallesEstadosTerminal] PRIMARY KEY  CLUSTERED 
	(
		[ID_EstadoTerminal],
		[NroTermino]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TMAR_Dispositivos] WITH NOCHECK ADD 
	CONSTRAINT [PK_TMAR_Dispositivos] PRIMARY KEY  CLUSTERED 
	(
		[ID_Dispositivo]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TMAR_DispositivosTerminal] WITH NOCHECK ADD 
	CONSTRAINT [PK_TMAR_DispositivosTerminal] PRIMARY KEY  CLUSTERED 
	(
		[ID_Terminal],
		[ID_Dispositivo]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TMAR_DispositivosTerminales_Hist] WITH NOCHECK ADD 
	CONSTRAINT [PK_TMAR_DispositivosTerminales_Hist] PRIMARY KEY  CLUSTERED 
	(
		[ID_Terminal],
		[ID_Dispositivo],
		[FechaInicio]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TMAR_DispositivosTipoTerminal] WITH NOCHECK ADD 
	CONSTRAINT [PK_TMAR_DispositivosTipoTerminal] PRIMARY KEY  CLUSTERED 
	(
		[ID_TipoTerminal],
		[ID_Dispositivo]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TMAR_EstadosDispositivos] WITH NOCHECK ADD 
	CONSTRAINT [PK_TMAR_EstadosDispositivos] PRIMARY KEY  CLUSTERED 
	(
		[ID_Dispositivo],
		[ID_Estado]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TMAR_EstadosTerminales] WITH NOCHECK ADD 
	CONSTRAINT [PK_TMAR_EstadosTerminales] PRIMARY KEY  CLUSTERED 
	(
		[ID_EstadoTerminal]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TMAR_EventosExternos] WITH NOCHECK ADD 
	CONSTRAINT [PK_TMAR_EventosExternos] PRIMARY KEY  CLUSTERED 
	(
		[ID_Proceso],
		[Mensaje]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TMAR_InfoTerminales] WITH NOCHECK ADD 
	CONSTRAINT [PK_TMAR_Terminales] PRIMARY KEY  CLUSTERED 
	(
		[ID_Terminal]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TMAR_InfoTerminales_Hist] WITH NOCHECK ADD 
	CONSTRAINT [PK_TMAR_EstadosTerminales_Hist] PRIMARY KEY  CLUSTERED 
	(
		[ID_Terminal],
		[FechaInicio]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TMAR_Procesos] WITH NOCHECK ADD 
	CONSTRAINT [TMAR_Procesos_PK] PRIMARY KEY  CLUSTERED 
	(
		[ID_Proceso]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TMAR_Severidad] WITH NOCHECK ADD 
	CONSTRAINT [TMAR_Severidad_PK] PRIMARY KEY  CLUSTERED 
	(
		[ID_Severidad]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TMAR_TipoTerminales] WITH NOCHECK ADD 
	CONSTRAINT [PK_TMAR_TipoTerminales] PRIMARY KEY  CLUSTERED 
	(
		[ID_TipoTerminal]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TMAR_DispositivosTerminal] ADD 
	CONSTRAINT [DF__TMAR_Disp__Fecha__0FEC5ADD] DEFAULT (getdate()) FOR [FechaEstado]
GO

ALTER TABLE [dbo].[TMAR_DetallesEstadosTerminal] ADD 
	CONSTRAINT [TMAR_Dispositivos_TMAR_DetallesEstadosTerminal_FK1] FOREIGN KEY 
	(
		[ID_Dispositivo]
	) REFERENCES [dbo].[TMAR_Dispositivos] (
		[ID_Dispositivo]
	),
	CONSTRAINT [TMAR_EstadosDispositivos_TMAR_DetallesEstadosTerminal_FK1] FOREIGN KEY 
	(
		[ID_Dispositivo],
		[ID_Estado]
	) REFERENCES [dbo].[TMAR_EstadosDispositivos] (
		[ID_Dispositivo],
		[ID_Estado]
	),
	CONSTRAINT [TMAR_EstadosTerminales_TMAR_DetallesEstadosTerminal_FK1] FOREIGN KEY 
	(
		[ID_EstadoTerminal]
	) REFERENCES [dbo].[TMAR_EstadosTerminales] (
		[ID_EstadoTerminal]
	)
GO

ALTER TABLE [dbo].[TMAR_DispositivosTerminal] ADD 
	CONSTRAINT [TMAR_Dispositivos_TMAR_DispositivosTerminal_FK1] FOREIGN KEY 
	(
		[ID_Dispositivo]
	) REFERENCES [dbo].[TMAR_Dispositivos] (
		[ID_Dispositivo]
	),
	CONSTRAINT [TMAR_EstadosDispositivos_TMAR_DispositivosTerminal_FK1] FOREIGN KEY 
	(
		[ID_Dispositivo],
		[ID_Estado]
	) REFERENCES [dbo].[TMAR_EstadosDispositivos] (
		[ID_Dispositivo],
		[ID_Estado]
	),
	CONSTRAINT [TMAR_Terminales_TMAR_DispositivosTerminal_FK1] FOREIGN KEY 
	(
		[ID_Terminal]
	) REFERENCES [dbo].[TMAR_InfoTerminales] (
		[ID_Terminal]
	)
GO

ALTER TABLE [dbo].[TMAR_DispositivosTerminales_Hist] ADD 
	CONSTRAINT [TMAR_Dispositivos_TMAR_DispositivosTerminales_Hist_FK1] FOREIGN KEY 
	(
		[ID_Dispositivo]
	) REFERENCES [dbo].[TMAR_Dispositivos] (
		[ID_Dispositivo]
	),
	CONSTRAINT [TMAR_EstadosTerminales_Hist_TMAR_DispositivosTerminales_Hist_FK1] FOREIGN KEY 
	(
		[ID_Terminal],
		[FechaInicio]
	) REFERENCES [dbo].[TMAR_InfoTerminales_Hist] (
		[ID_Terminal],
		[FechaInicio]
	),
	CONSTRAINT [TMAR_Terminales_TMAR_DispositivosTerminales_Hist_FK1] FOREIGN KEY 
	(
		[ID_Terminal]
	) REFERENCES [dbo].[TMAR_InfoTerminales] (
		[ID_Terminal]
	)
GO

ALTER TABLE [dbo].[TMAR_DispositivosTipoTerminal] ADD 
	CONSTRAINT [TMAR_Dispositivos_TMAR_DispositivosTipoTerminal_FK1] FOREIGN KEY 
	(
		[ID_Dispositivo]
	) REFERENCES [dbo].[TMAR_Dispositivos] (
		[ID_Dispositivo]
	),
	CONSTRAINT [TMAR_TipoTerminales_TMAR_DispositivosTipoTerminal_FK1] FOREIGN KEY 
	(
		[ID_TipoTerminal]
	) REFERENCES [dbo].[TMAR_TipoTerminales] (
		[ID_TipoTerminal]
	)
GO

ALTER TABLE [dbo].[TMAR_EstadosDispositivos] ADD 
	CONSTRAINT [TMAR_Dispositivos_TMAR_EstadosDispositivos_FK1] FOREIGN KEY 
	(
		[ID_Dispositivo]
	) REFERENCES [dbo].[TMAR_Dispositivos] (
		[ID_Dispositivo]
	),
	CONSTRAINT [TMAR_Severidad_TMAR_EstadosDispositivos_FK1] FOREIGN KEY 
	(
		[ID_Severidad]
	) REFERENCES [dbo].[TMAR_Severidad] (
		[ID_Severidad]
	)
GO

ALTER TABLE [dbo].[TMAR_EstadosTerminales] ADD 
	CONSTRAINT [TMAR_Severidad_TMAR_EstadosTerminales_FK1] FOREIGN KEY 
	(
		[ID_Severidad]
	) REFERENCES [dbo].[TMAR_Severidad] (
		[ID_Severidad]
	),
	CONSTRAINT [TMAR_TipoTerminales_TMAR_EstadosTerminales_FK1] FOREIGN KEY 
	(
		[ID_TipoTerminal]
	) REFERENCES [dbo].[TMAR_TipoTerminales] (
		[ID_TipoTerminal]
	)
GO

ALTER TABLE [dbo].[TMAR_EventosExternos] ADD 
	CONSTRAINT [TMAR_EstadosDispositivos_TMAR_EventosExternos_FK1] FOREIGN KEY 
	(
		[ID_Dispositivo],
		[ID_Estado]
	) REFERENCES [dbo].[TMAR_EstadosDispositivos] (
		[ID_Dispositivo],
		[ID_Estado]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [TMAR_Procesos_TMAR_EventosExternos_FK1] FOREIGN KEY 
	(
		[ID_Proceso]
	) REFERENCES [dbo].[TMAR_Procesos] (
		[ID_Proceso]
	)
GO

ALTER TABLE [dbo].[TMAR_InfoTerminales] ADD 
	CONSTRAINT [TMAR_Dispositivos_TMAR_Terminales_FK1] FOREIGN KEY 
	(
		[ID_DispositivoCausa]
	) REFERENCES [dbo].[TMAR_Dispositivos] (
		[ID_Dispositivo]
	),
	CONSTRAINT [TMAR_EstadosTerminales_TMAR_Terminales_FK1] FOREIGN KEY 
	(
		[ID_EstadoTerminal]
	) REFERENCES [dbo].[TMAR_EstadosTerminales] (
		[ID_EstadoTerminal]
	),
	CONSTRAINT [TMAR_TipoTerminales_TMAR_Terminales_FK1] FOREIGN KEY 
	(
		[ID_TipoTerminal]
	) REFERENCES [dbo].[TMAR_TipoTerminales] (
		[ID_TipoTerminal]
	)
GO

ALTER TABLE [dbo].[TMAR_InfoTerminales_Hist] ADD 
	CONSTRAINT [TMAR_Dispositivos_TMAR_InfoTerminales_Hist_FK1] FOREIGN KEY 
	(
		[ID_DispositivoCausa]
	) REFERENCES [dbo].[TMAR_Dispositivos] (
		[ID_Dispositivo]
	),
	CONSTRAINT [TMAR_EstadosTerminales_TMAR_EstadosTerminales_Hist_FK1] FOREIGN KEY 
	(
		[ID_EstadoTerminal]
	) REFERENCES [dbo].[TMAR_EstadosTerminales] (
		[ID_EstadoTerminal]
	),
	CONSTRAINT [TMAR_Terminales_TMAR_EstadosTerminales_Hist_FK1] FOREIGN KEY 
	(
		[ID_Terminal]
	) REFERENCES [dbo].[TMAR_InfoTerminales] (
		[ID_Terminal]
	)
GO

ALTER TABLE [dbo].[TMAR_TipoTerminales] ADD 
	CONSTRAINT [TMAR_Procesos_TMAR_TipoTerminales_FK1] FOREIGN KEY 
	(
		[ID_Proceso]
	) REFERENCES [dbo].[TMAR_Procesos] (
		[ID_Proceso]
	)
GO

