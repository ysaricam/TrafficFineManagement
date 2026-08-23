BEGIN;

ALTER TABLE traffic_fines."Fines"
    ADD COLUMN IF NOT EXISTS "CurrentAction" integer NOT NULL DEFAULT 0;

COMMIT;
