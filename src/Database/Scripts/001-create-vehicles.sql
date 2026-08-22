BEGIN;

CREATE SCHEMA IF NOT EXISTS vehicles;

CREATE TABLE IF NOT EXISTS vehicles."Vehicles"
(
    "Id" uuid NOT NULL,
    "Plaka" text NOT NULL,
    "Brand" text NOT NULL,
    "Model" text NOT NULL,
    "Status" boolean NOT NULL DEFAULT FALSE,
    CONSTRAINT "PK_Vehicles" PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS vehicles."VehicleUsers"
(
    "VehicleId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "StartTime" timestamp with time zone NOT NULL,
    "EndTime" timestamp with time zone NULL,
    CONSTRAINT "PK_VehicleUsers" PRIMARY KEY ("VehicleId", "UserId"),
    CONSTRAINT "FK_VehicleUsers_Vehicles_VehicleId"
        FOREIGN KEY ("VehicleId")
        REFERENCES vehicles."Vehicles" ("Id")
        ON DELETE CASCADE
);

COMMIT;
