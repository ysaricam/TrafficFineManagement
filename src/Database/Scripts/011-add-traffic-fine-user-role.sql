BEGIN;

ALTER TABLE traffic_fines."Users"
    ADD COLUMN IF NOT EXISTS "Role" integer NOT NULL DEFAULT 0;

DO $$
BEGIN
    IF NOT EXISTS
    (
        SELECT 1
        FROM pg_constraint
        WHERE conname = 'CK_TrafficFine_Users_Role'
          AND connamespace = 'traffic_fines'::regnamespace
    ) THEN
        ALTER TABLE traffic_fines."Users"
            ADD CONSTRAINT "CK_TrafficFine_Users_Role"
            CHECK ("Role" BETWEEN 0 AND 3);
    END IF;
END
$$;

COMMIT;
