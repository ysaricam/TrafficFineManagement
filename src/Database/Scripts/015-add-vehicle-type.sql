BEGIN;

ALTER TABLE vehicles."Vehicles"
    ADD COLUMN IF NOT EXISTS "Type" integer NOT NULL DEFAULT 0;

DO $$
BEGIN
    IF NOT EXISTS
    (
        SELECT 1
        FROM pg_constraint
        WHERE conname = 'CK_Vehicles_Type'
          AND connamespace = 'vehicles'::regnamespace
    ) THEN
        ALTER TABLE vehicles."Vehicles"
            ADD CONSTRAINT "CK_Vehicles_Type"
            CHECK ("Type" BETWEEN 0 AND 3);
    END IF;
END
$$;

CREATE OR REPLACE VIEW vehicles."VehicleReadModel" AS
SELECT
    vehicle."Id",
    vehicle."Plaka",
    vehicle."Brand",
    vehicle."Model",
    vehicle."Status",
    vehicle_user."UserId",
    vehicle_user."StartTime",
    vehicle_user."EndTime",
    vehicle."Type"
FROM vehicles."Vehicles" AS vehicle
LEFT JOIN vehicles."VehicleUsers" AS vehicle_user
    ON vehicle_user."VehicleId" = vehicle."Id";

COMMIT;
