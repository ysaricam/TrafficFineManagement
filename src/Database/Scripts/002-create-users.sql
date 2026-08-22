BEGIN;

CREATE TABLE IF NOT EXISTS vehicles."Users"
(
    "Id" uuid NOT NULL,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);

DO $$
BEGIN
    IF NOT EXISTS
    (
        SELECT 1
        FROM pg_constraint
        WHERE conname = 'FK_VehicleUsers_Users_UserId'
          AND connamespace = 'vehicles'::regnamespace
    ) THEN
        ALTER TABLE vehicles."VehicleUsers"
            ADD CONSTRAINT "FK_VehicleUsers_Users_UserId"
            FOREIGN KEY ("UserId")
            REFERENCES vehicles."Users" ("Id")
            ON DELETE RESTRICT;
    END IF;
END
$$;

COMMIT;
