BEGIN;

CREATE TABLE IF NOT EXISTS traffic_fines."OutboxMessages"
(
    "Id" uuid NOT NULL,
    "OccurredOn" timestamp with time zone NOT NULL,
    "Type" character varying(500) NOT NULL,
    "Data" jsonb NOT NULL,
    "ProcessedDate" timestamp with time zone NULL,
    CONSTRAINT "PK_TrafficFine_OutboxMessages" PRIMARY KEY ("Id")
);

CREATE INDEX IF NOT EXISTS "IX_TrafficFine_OutboxMessages_ProcessedDate_OccurredOn"
    ON traffic_fines."OutboxMessages" ("ProcessedDate", "OccurredOn");

COMMIT;
