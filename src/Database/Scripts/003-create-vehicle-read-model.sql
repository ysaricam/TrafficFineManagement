BEGIN;

CREATE OR REPLACE VIEW vehicles."VehicleReadModel" AS
SELECT
    vehicle."Id",
    vehicle."Plaka",
    vehicle."Brand",
    vehicle."Model",
    vehicle."Status",
    vehicle_user."UserId",
    vehicle_user."StartTime",
    vehicle_user."EndTime"
FROM vehicles."Vehicles" AS vehicle
LEFT JOIN vehicles."VehicleUsers" AS vehicle_user
    ON vehicle_user."VehicleId" = vehicle."Id";

COMMIT;
