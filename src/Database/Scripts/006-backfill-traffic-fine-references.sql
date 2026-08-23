BEGIN;

INSERT INTO traffic_fines."Users" ("Id")
SELECT "Id"
FROM vehicles."Users"
ON CONFLICT ("Id") DO NOTHING;

INSERT INTO traffic_fines."Vehicles" ("Id")
SELECT "Id"
FROM vehicles."Vehicles"
ON CONFLICT ("Id") DO NOTHING;

COMMIT;
