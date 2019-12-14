--------------------------------------------------------
-- Archivo creado  - viernes-diciembre-06-2019   
--------------------------------------------------------
--------------------------------------------------------
--  DDL for Sequence TABLE1_SEQ
--------------------------------------------------------

   CREATE SEQUENCE  "ADMIN"."TABLE1_SEQ"  MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 1 CACHE 20 NOORDER  NOCYCLE  NOKEEP  NOSCALE  GLOBAL ;
--------------------------------------------------------
--  DDL for Table BLOQUE
--------------------------------------------------------

  CREATE TABLE "ADMIN"."BLOQUE" 
   (	"ID" NUMBER GENERATED ALWAYS AS IDENTITY MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 1 CACHE 20 NOORDER  NOCYCLE  NOKEEP  NOSCALE , 
	"DESCRIPCION" VARCHAR2(300 BYTE)
   ) SEGMENT CREATION DEFERRED 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  TABLESPACE "USERS" ;
--------------------------------------------------------
--  DDL for Table ESPACIO
--------------------------------------------------------

  CREATE TABLE "ADMIN"."ESPACIO" 
   (	"ID" NUMBER GENERATED ALWAYS AS IDENTITY MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 1 CACHE 20 NOORDER  NOCYCLE  NOKEEP  NOSCALE , 
	"DESCRIPCION" VARCHAR2(300 BYTE), 
	"BLOQUE" NUMBER
   ) SEGMENT CREATION DEFERRED 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  TABLESPACE "USERS" ;
--------------------------------------------------------
--  DDL for Table ESTADO_INVITACION
--------------------------------------------------------

  CREATE TABLE "ADMIN"."ESTADO_INVITACION" 
   (	"ID" NUMBER GENERATED ALWAYS AS IDENTITY MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 1 CACHE 20 NOORDER  NOCYCLE  NOKEEP  NOSCALE , 
	"NOMBRE" VARCHAR2(50 BYTE), 
	"ABREVIATURA" VARCHAR2(3 BYTE)
   ) SEGMENT CREATION DEFERRED 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  TABLESPACE "USERS" ;
--------------------------------------------------------
--  DDL for Table FILTRO
--------------------------------------------------------

  CREATE TABLE "ADMIN"."FILTRO" 
   (	"ID" NUMBER GENERATED ALWAYS AS IDENTITY MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 1 CACHE 20 NOORDER  NOCYCLE  NOKEEP  NOSCALE , 
	"TIPO_FILTRO" NUMBER
   ) SEGMENT CREATION DEFERRED 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  TABLESPACE "USERS" ;
--------------------------------------------------------
--  DDL for Table INVITACION
--------------------------------------------------------

  CREATE TABLE "ADMIN"."INVITACION" 
   (	"ID" NUMBER GENERATED ALWAYS AS IDENTITY MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 1 CACHE 20 NOORDER  NOCYCLE  NOKEEP  NOSCALE , 
	"REUNIONID" NUMBER, 
	"USERNAMEINVITADO" VARCHAR2(100 BYTE), 
	"ESTADO" NUMBER, 
	"ELIMINADO" VARCHAR2(1 BYTE) DEFAULT 0
   ) SEGMENT CREATION DEFERRED 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  TABLESPACE "USERS" ;
--------------------------------------------------------
--  DDL for Table LISTA_PERSONALIZADA
--------------------------------------------------------

  CREATE TABLE "ADMIN"."LISTA_PERSONALIZADA" 
   (	"ID" NUMBER GENERATED ALWAYS AS IDENTITY MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 1 CACHE 20 NOORDER  NOCYCLE  NOKEEP  NOSCALE , 
	"USUARIO" VARCHAR2(100 BYTE), 
	"NOMBRE" VARCHAR2(200 BYTE), 
	"FECHA_CREACION" DATE
   ) SEGMENT CREATION DEFERRED 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  TABLESPACE "USERS" ;
--------------------------------------------------------
--  DDL for Table NOMBRE_BLOQUE
--------------------------------------------------------

  CREATE TABLE "ADMIN"."NOMBRE_BLOQUE" 
   (	"ID" NUMBER GENERATED ALWAYS AS IDENTITY MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 1 CACHE 20 NOORDER  NOCYCLE  NOKEEP  NOSCALE , 
	"NOMBRE" VARCHAR2(100 BYTE)
   ) SEGMENT CREATION DEFERRED 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  TABLESPACE "USERS" ;
--------------------------------------------------------
--  DDL for Table NOMBRE_ESPACIO
--------------------------------------------------------

  CREATE TABLE "ADMIN"."NOMBRE_ESPACIO" 
   (	"ID" NUMBER GENERATED ALWAYS AS IDENTITY MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 1 CACHE 20 NOORDER  NOCYCLE  NOKEEP  NOSCALE , 
	"NOMBRE" VARCHAR2(100 BYTE), 
	"ESPACIOID" NUMBER
   ) SEGMENT CREATION DEFERRED 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  TABLESPACE "USERS" ;
--------------------------------------------------------
--  DDL for Table REUNION
--------------------------------------------------------

  CREATE TABLE "ADMIN"."REUNION" 
   (	"ID" NUMBER GENERATED ALWAYS AS IDENTITY MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 1 CACHE 20 NOORDER  NOCYCLE  NOKEEP  NOSCALE , 
	"USERNAMEORGANIZADOR" VARCHAR2(100 BYTE), 
	"ELIMINADO" VARCHAR2(1 BYTE) DEFAULT 0
   ) SEGMENT CREATION DEFERRED 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  TABLESPACE "USERS" ;
--------------------------------------------------------
--  DDL for Table TIPO_ESPACIO
--------------------------------------------------------

  CREATE TABLE "ADMIN"."TIPO_ESPACIO" 
   (	"ID" NUMBER GENERATED ALWAYS AS IDENTITY MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 1 CACHE 20 NOORDER  NOCYCLE  NOKEEP  NOSCALE , 
	"NOMBRE" VARCHAR2(100 BYTE)
   ) SEGMENT CREATION DEFERRED 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  TABLESPACE "USERS" ;
--------------------------------------------------------
--  DDL for Table TIPO_FILTRO
--------------------------------------------------------

  CREATE TABLE "ADMIN"."TIPO_FILTRO" 
   (	"ID" NUMBER GENERATED ALWAYS AS IDENTITY MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 1 CACHE 20 NOORDER  NOCYCLE  NOKEEP  NOSCALE , 
	"CRITERIO" VARCHAR2(200 BYTE)
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "USERS" ;
--------------------------------------------------------
--  DDL for Index BLOQUE_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ADMIN"."BLOQUE_PK" ON "ADMIN"."BLOQUE" ("ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "USERS" ;
--------------------------------------------------------
--  DDL for Index ESPACIO_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ADMIN"."ESPACIO_PK" ON "ADMIN"."ESPACIO" ("ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "USERS" ;
--------------------------------------------------------
--  DDL for Index ESTADOINVITACION_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ADMIN"."ESTADOINVITACION_PK" ON "ADMIN"."ESTADO_INVITACION" ("ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "USERS" ;
--------------------------------------------------------
--  DDL for Index FILTRO_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ADMIN"."FILTRO_PK" ON "ADMIN"."FILTRO" ("ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "USERS" ;
--------------------------------------------------------
--  DDL for Index LISTA_PERSONALIZADA_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ADMIN"."LISTA_PERSONALIZADA_PK" ON "ADMIN"."LISTA_PERSONALIZADA" ("ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "USERS" ;
--------------------------------------------------------
--  DDL for Index NOMBRE_BLOQUE_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ADMIN"."NOMBRE_BLOQUE_PK" ON "ADMIN"."NOMBRE_BLOQUE" ("ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "USERS" ;
--------------------------------------------------------
--  DDL for Index NOMBRE_ESPACIO_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ADMIN"."NOMBRE_ESPACIO_PK" ON "ADMIN"."NOMBRE_ESPACIO" ("ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "USERS" ;
--------------------------------------------------------
--  DDL for Index REUNION_INVITADO_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ADMIN"."REUNION_INVITADO_PK" ON "ADMIN"."INVITACION" ("ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "USERS" ;
--------------------------------------------------------
--  DDL for Index REUNION_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ADMIN"."REUNION_PK" ON "ADMIN"."REUNION" ("ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "USERS" ;
--------------------------------------------------------
--  DDL for Index TIPO_ESPACIO_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ADMIN"."TIPO_ESPACIO_PK" ON "ADMIN"."TIPO_ESPACIO" ("ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "USERS" ;
--------------------------------------------------------
--  DDL for Index TIPO_FILTRO_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "ADMIN"."TIPO_FILTRO_PK" ON "ADMIN"."TIPO_FILTRO" ("ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "USERS" ;
--------------------------------------------------------
--  Constraints for Table REUNION
--------------------------------------------------------

  ALTER TABLE "ADMIN"."REUNION" MODIFY ("ID" NOT NULL ENABLE);
  ALTER TABLE "ADMIN"."REUNION" ADD CONSTRAINT "REUNION_PK" PRIMARY KEY ("ID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "USERS"  ENABLE;
  ALTER TABLE "ADMIN"."REUNION" MODIFY ("USERNAMEORGANIZADOR" NOT NULL ENABLE);
  ALTER TABLE "ADMIN"."REUNION" MODIFY ("ELIMINADO" NOT NULL ENABLE);
--------------------------------------------------------
--  Constraints for Table LISTA_PERSONALIZADA
--------------------------------------------------------

  ALTER TABLE "ADMIN"."LISTA_PERSONALIZADA" MODIFY ("ID" NOT NULL ENABLE);
  ALTER TABLE "ADMIN"."LISTA_PERSONALIZADA" MODIFY ("USUARIO" NOT NULL ENABLE);
  ALTER TABLE "ADMIN"."LISTA_PERSONALIZADA" MODIFY ("NOMBRE" NOT NULL ENABLE);
  ALTER TABLE "ADMIN"."LISTA_PERSONALIZADA" MODIFY ("FECHA_CREACION" NOT NULL ENABLE);
  ALTER TABLE "ADMIN"."LISTA_PERSONALIZADA" ADD CONSTRAINT "LISTA_PERSONALIZADA_PK" PRIMARY KEY ("ID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "USERS"  ENABLE;
--------------------------------------------------------
--  Constraints for Table ESPACIO
--------------------------------------------------------

  ALTER TABLE "ADMIN"."ESPACIO" MODIFY ("ID" NOT NULL ENABLE);
  ALTER TABLE "ADMIN"."ESPACIO" ADD CONSTRAINT "ESPACIO_PK" PRIMARY KEY ("ID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "USERS"  ENABLE;
  ALTER TABLE "ADMIN"."ESPACIO" MODIFY ("BLOQUE" NOT NULL ENABLE);
--------------------------------------------------------
--  Constraints for Table FILTRO
--------------------------------------------------------

  ALTER TABLE "ADMIN"."FILTRO" MODIFY ("ID" NOT NULL ENABLE);
  ALTER TABLE "ADMIN"."FILTRO" ADD CONSTRAINT "FILTRO_PK" PRIMARY KEY ("ID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "USERS"  ENABLE;
  ALTER TABLE "ADMIN"."FILTRO" MODIFY ("TIPO_FILTRO" NOT NULL ENABLE);
--------------------------------------------------------
--  Constraints for Table BLOQUE
--------------------------------------------------------

  ALTER TABLE "ADMIN"."BLOQUE" MODIFY ("ID" NOT NULL ENABLE);
  ALTER TABLE "ADMIN"."BLOQUE" ADD CONSTRAINT "BLOQUE_PK" PRIMARY KEY ("ID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "USERS"  ENABLE;
--------------------------------------------------------
--  Constraints for Table TIPO_FILTRO
--------------------------------------------------------

  ALTER TABLE "ADMIN"."TIPO_FILTRO" MODIFY ("ID" NOT NULL ENABLE);
  ALTER TABLE "ADMIN"."TIPO_FILTRO" ADD CONSTRAINT "TIPO_FILTRO_PK" PRIMARY KEY ("ID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "USERS"  ENABLE;
  ALTER TABLE "ADMIN"."TIPO_FILTRO" MODIFY ("CRITERIO" NOT NULL ENABLE);
--------------------------------------------------------
--  Constraints for Table TIPO_ESPACIO
--------------------------------------------------------

  ALTER TABLE "ADMIN"."TIPO_ESPACIO" MODIFY ("ID" NOT NULL ENABLE);
  ALTER TABLE "ADMIN"."TIPO_ESPACIO" MODIFY ("NOMBRE" NOT NULL ENABLE);
  ALTER TABLE "ADMIN"."TIPO_ESPACIO" ADD CONSTRAINT "TIPO_ESPACIO_PK" PRIMARY KEY ("ID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "USERS"  ENABLE;
--------------------------------------------------------
--  Constraints for Table ESTADO_INVITACION
--------------------------------------------------------

  ALTER TABLE "ADMIN"."ESTADO_INVITACION" MODIFY ("ID" NOT NULL ENABLE);
  ALTER TABLE "ADMIN"."ESTADO_INVITACION" MODIFY ("NOMBRE" NOT NULL ENABLE);
  ALTER TABLE "ADMIN"."ESTADO_INVITACION" ADD CONSTRAINT "ESTADOINVITACION_PK" PRIMARY KEY ("ID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "USERS"  ENABLE;
--------------------------------------------------------
--  Constraints for Table NOMBRE_BLOQUE
--------------------------------------------------------

  ALTER TABLE "ADMIN"."NOMBRE_BLOQUE" MODIFY ("ID" NOT NULL ENABLE);
  ALTER TABLE "ADMIN"."NOMBRE_BLOQUE" MODIFY ("NOMBRE" NOT NULL ENABLE);
  ALTER TABLE "ADMIN"."NOMBRE_BLOQUE" ADD CONSTRAINT "NOMBRE_BLOQUE_PK" PRIMARY KEY ("ID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "USERS"  ENABLE;
--------------------------------------------------------
--  Constraints for Table NOMBRE_ESPACIO
--------------------------------------------------------

  ALTER TABLE "ADMIN"."NOMBRE_ESPACIO" MODIFY ("ID" NOT NULL ENABLE);
  ALTER TABLE "ADMIN"."NOMBRE_ESPACIO" MODIFY ("NOMBRE" NOT NULL ENABLE);
  ALTER TABLE "ADMIN"."NOMBRE_ESPACIO" MODIFY ("ESPACIOID" NOT NULL ENABLE);
  ALTER TABLE "ADMIN"."NOMBRE_ESPACIO" ADD CONSTRAINT "NOMBRE_ESPACIO_PK" PRIMARY KEY ("ID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "USERS"  ENABLE;
--------------------------------------------------------
--  Constraints for Table INVITACION
--------------------------------------------------------

  ALTER TABLE "ADMIN"."INVITACION" MODIFY ("ID" NOT NULL ENABLE);
  ALTER TABLE "ADMIN"."INVITACION" MODIFY ("REUNIONID" NOT NULL ENABLE);
  ALTER TABLE "ADMIN"."INVITACION" MODIFY ("USERNAMEINVITADO" NOT NULL ENABLE);
  ALTER TABLE "ADMIN"."INVITACION" ADD CONSTRAINT "REUNION_INVITADO_PK" PRIMARY KEY ("ID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "USERS"  ENABLE;
  ALTER TABLE "ADMIN"."INVITACION" MODIFY ("ESTADO" NOT NULL ENABLE);
  ALTER TABLE "ADMIN"."INVITACION" MODIFY ("ELIMINADO" NOT NULL ENABLE);
--------------------------------------------------------
--  Ref Constraints for Table ESPACIO
--------------------------------------------------------

  ALTER TABLE "ADMIN"."ESPACIO" ADD CONSTRAINT "ESPACIO_FK1" FOREIGN KEY ("BLOQUE")
	  REFERENCES "ADMIN"."BLOQUE" ("ID") ENABLE;
--------------------------------------------------------
--  Ref Constraints for Table FILTRO
--------------------------------------------------------

  ALTER TABLE "ADMIN"."FILTRO" ADD CONSTRAINT "FILTRO_FK1" FOREIGN KEY ("TIPO_FILTRO")
	  REFERENCES "ADMIN"."TIPO_FILTRO" ("ID") ENABLE;
--------------------------------------------------------
--  Ref Constraints for Table INVITACION
--------------------------------------------------------

  ALTER TABLE "ADMIN"."INVITACION" ADD CONSTRAINT "REUNION_INVITADO_FK1" FOREIGN KEY ("REUNIONID")
	  REFERENCES "ADMIN"."INVITACION" ("ID") ENABLE;
  ALTER TABLE "ADMIN"."INVITACION" ADD CONSTRAINT "REUNION_INVITADO_FK2" FOREIGN KEY ("ESTADO")
	  REFERENCES "ADMIN"."ESTADO_INVITACION" ("ID") ENABLE;
--------------------------------------------------------
--  Ref Constraints for Table NOMBRE_ESPACIO
--------------------------------------------------------

  ALTER TABLE "ADMIN"."NOMBRE_ESPACIO" ADD CONSTRAINT "NOMBRE_ESPACIO_FK1" FOREIGN KEY ("ESPACIOID")
	  REFERENCES "ADMIN"."ESPACIO" ("ID") ENABLE;