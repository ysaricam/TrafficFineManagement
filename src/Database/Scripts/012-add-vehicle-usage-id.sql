BEGIN;

ALTER TABLE vehicles."VehicleUsers"
    ADD COLUMN IF NOT EXISTS "Id" uuid;

UPDATE vehicles."VehicleUsers"
SET "Id" = gen_random_uuid()
WHERE "Id" IS NULL;

ALTER TABLE vehicles."VehicleUsers"
    ALTER COLUMN "Id" SET NOT NULL;

ALTER TABLE vehicles."VehicleUsers"
    DROP CONSTRAINT IF EXISTS "PK_VehicleUsers";

ALTER TABLE vehicles."VehicleUsers"
    ADD CONSTRAINT "PK_VehicleUsers"
    PRIMARY KEY ("VehicleId", "Id");

CREATE INDEX IF NOT EXISTS "IX_VehicleUsers_VehicleId_UserId"
    ON vehicles."VehicleUsers" ("VehicleId", "UserId");

COMMIT;
