BEGIN;

CREATE SCHEMA IF NOT EXISTS users;

CREATE TABLE IF NOT EXISTS users."Users"
(
    "Id" uuid NOT NULL,
    "Name" character varying(100) NOT NULL,
    "Surname" character varying(100) NOT NULL,
    "Username" character varying(50) NOT NULL,
    "PasswordHash" character varying(500) NOT NULL,
    "Role" integer NOT NULL,
    CONSTRAINT "PK_UsersModule_Users" PRIMARY KEY ("Id"),
    CONSTRAINT "CK_UsersModule_Users_Role" CHECK ("Role" BETWEEN 0 AND 3)
);

CREATE UNIQUE INDEX IF NOT EXISTS "UX_UsersModule_Users_Username"
    ON users."Users" ("Username");

CREATE TABLE IF NOT EXISTS users."OutboxMessages"
(
    "Id" uuid NOT NULL,
    "OccurredOn" timestamp with time zone NOT NULL,
    "Type" character varying(500) NOT NULL,
    "Data" jsonb NOT NULL,
    "ProcessedDate" timestamp with time zone NULL,
    CONSTRAINT "PK_UsersModule_OutboxMessages" PRIMARY KEY ("Id")
);

CREATE INDEX IF NOT EXISTS "IX_UsersModule_OutboxMessages_ProcessedDate_OccurredOn"
    ON users."OutboxMessages" ("ProcessedDate", "OccurredOn");

COMMIT;
