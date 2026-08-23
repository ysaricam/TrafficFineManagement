BEGIN;

CREATE TABLE IF NOT EXISTS vehicles."OutboxMessages"
(
    "Id" uuid NOT NULL,
    "OccurredOn" timestamp with time zone NOT NULL,
    "Type" character varying(500) NOT NULL,
    "Data" jsonb NOT NULL,
    "ProcessedDate" timestamp with time zone NULL,
    CONSTRAINT "PK_OutboxMessages" PRIMARY KEY ("Id")
);

CREATE INDEX IF NOT EXISTS "IX_OutboxMessages_ProcessedDate_OccurredOn"
    ON vehicles."OutboxMessages" ("ProcessedDate", "OccurredOn");

COMMIT;
