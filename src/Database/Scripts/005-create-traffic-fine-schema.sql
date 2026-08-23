BEGIN;

CREATE SCHEMA IF NOT EXISTS traffic_fines;

CREATE TABLE IF NOT EXISTS traffic_fines."Users"
(
    "Id" uuid NOT NULL,
    CONSTRAINT "PK_TrafficFine_Users" PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS traffic_fines."Vehicles"
(
    "Id" uuid NOT NULL,
    CONSTRAINT "PK_TrafficFine_Vehicles" PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS traffic_fines."Fines"
(
    "Id" uuid NOT NULL,
    "FinedUserId" uuid NOT NULL,
    "VehicleId" uuid NOT NULL,
    "Amount" numeric(18, 2) NOT NULL,
    "Currency" character varying(3) NOT NULL,
    "ViolationCode" character varying(50) NOT NULL,
    "Reason" character varying(1000) NOT NULL,
    "FineDate" timestamp with time zone NOT NULL,
    "Status" integer NOT NULL,
    CONSTRAINT "PK_Fines" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Fines_Users_FinedUserId"
        FOREIGN KEY ("FinedUserId")
        REFERENCES traffic_fines."Users" ("Id")
        ON DELETE RESTRICT,
    CONSTRAINT "FK_Fines_Vehicles_VehicleId"
        FOREIGN KEY ("VehicleId")
        REFERENCES traffic_fines."Vehicles" ("Id")
        ON DELETE RESTRICT
);

CREATE TABLE IF NOT EXISTS traffic_fines."FineApprovalHistories"
(
    "Id" uuid NOT NULL,
    "FineId" uuid NOT NULL,
    "PerformedByUserId" uuid NOT NULL,
    "ActionDate" timestamp with time zone NOT NULL,
    "ActionType" integer NOT NULL,
    "Description" character varying(1000) NULL,
    "PreviousStatus" integer NOT NULL,
    "NewStatus" integer NOT NULL,
    CONSTRAINT "PK_FineApprovalHistories" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_FineApprovalHistories_Fines_FineId"
        FOREIGN KEY ("FineId")
        REFERENCES traffic_fines."Fines" ("Id")
        ON DELETE CASCADE,
    CONSTRAINT "FK_FineApprovalHistories_Users_PerformedByUserId"
        FOREIGN KEY ("PerformedByUserId")
        REFERENCES traffic_fines."Users" ("Id")
        ON DELETE RESTRICT
);

CREATE INDEX IF NOT EXISTS "IX_Fines_FinedUserId"
    ON traffic_fines."Fines" ("FinedUserId");

CREATE INDEX IF NOT EXISTS "IX_Fines_VehicleId"
    ON traffic_fines."Fines" ("VehicleId");

CREATE INDEX IF NOT EXISTS "IX_Fines_FineDate"
    ON traffic_fines."Fines" ("FineDate");

CREATE INDEX IF NOT EXISTS "IX_FineApprovalHistories_FineId_ActionDate"
    ON traffic_fines."FineApprovalHistories" ("FineId", "ActionDate");

CREATE INDEX IF NOT EXISTS "IX_FineApprovalHistories_PerformedByUserId"
    ON traffic_fines."FineApprovalHistories" ("PerformedByUserId");

COMMIT;
