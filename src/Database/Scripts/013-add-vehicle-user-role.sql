BEGIN;

ALTER TABLE vehicles."Users"
    ADD COLUMN IF NOT EXISTS "Role" integer NOT NULL DEFAULT 0;

UPDATE vehicles."Users" AS vehicle_user
SET "Role" = source_user."Role"
FROM users."Users" AS source_user
WHERE vehicle_user."Id" = source_user."Id";

DO $$
BEGIN
    IF NOT EXISTS
    (
        SELECT 1
        FROM pg_constraint
        WHERE conname = 'CK_Vehicles_Users_Role'
          AND connamespace = 'vehicles'::regnamespace
    ) THEN
        ALTER TABLE vehicles."Users"
            ADD CONSTRAINT "CK_Vehicles_Users_Role"
            CHECK ("Role" BETWEEN 0 AND 3);
    END IF;
END
$$;

COMMIT;
