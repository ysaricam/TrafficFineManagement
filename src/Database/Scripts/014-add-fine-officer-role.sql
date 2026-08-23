BEGIN;

ALTER TABLE users."Users"
    DROP CONSTRAINT IF EXISTS "CK_UsersModule_Users_Role";
ALTER TABLE users."Users"
    ADD CONSTRAINT "CK_UsersModule_Users_Role"
    CHECK ("Role" BETWEEN 0 AND 4);

ALTER TABLE traffic_fines."Users"
    DROP CONSTRAINT IF EXISTS "CK_TrafficFine_Users_Role";
ALTER TABLE traffic_fines."Users"
    ADD CONSTRAINT "CK_TrafficFine_Users_Role"
    CHECK ("Role" BETWEEN 0 AND 4);

ALTER TABLE vehicles."Users"
    DROP CONSTRAINT IF EXISTS "CK_Vehicles_Users_Role";
ALTER TABLE vehicles."Users"
    ADD CONSTRAINT "CK_Vehicles_Users_Role"
    CHECK ("Role" BETWEEN 0 AND 4);

COMMIT;
