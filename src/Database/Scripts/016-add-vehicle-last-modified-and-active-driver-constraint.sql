BEGIN;

ALTER TABLE vehicles."Vehicles"
    ADD COLUMN IF NOT EXISTS "LastModifiedAt" timestamp with time zone
    NOT NULL DEFAULT CURRENT_TIMESTAMP;

UPDATE vehicles."Vehicles" AS vehicle
SET "LastModifiedAt" = usage."LastModifiedAt"
FROM
(
    SELECT
        "VehicleId",
        MAX(GREATEST("StartTime", COALESCE("EndTime", "StartTime"))) AS "LastModifiedAt"
    FROM vehicles."VehicleUsers"
    GROUP BY "VehicleId"
) AS usage
WHERE usage."VehicleId" = vehicle."Id";

WITH active_usages AS
(
    SELECT
        "VehicleId",
        "Id",
        "StartTime",
        MAX("StartTime") OVER (PARTITION BY "UserId") AS "LatestStartTime",
        ROW_NUMBER() OVER
        (
            PARTITION BY "UserId"
            ORDER BY "StartTime" DESC, "Id" DESC
        ) AS "RowNumber"
    FROM vehicles."VehicleUsers"
    WHERE "EndTime" IS NULL
)
UPDATE vehicles."VehicleUsers" AS vehicle_user
SET "EndTime" = GREATEST(
    active_usage."StartTime",
    active_usage."LatestStartTime")
FROM active_usages AS active_usage
WHERE vehicle_user."VehicleId" = active_usage."VehicleId"
  AND vehicle_user."Id" = active_usage."Id"
  AND active_usage."RowNumber" > 1;

UPDATE vehicles."Vehicles" AS vehicle
SET "Status" = EXISTS
(
    SELECT 1
    FROM vehicles."VehicleUsers" AS vehicle_user
    WHERE vehicle_user."VehicleId" = vehicle."Id"
      AND vehicle_user."EndTime" IS NULL
);

CREATE UNIQUE INDEX IF NOT EXISTS "UX_VehicleUsers_ActiveUserId"
    ON vehicles."VehicleUsers" ("UserId")
    WHERE "EndTime" IS NULL;

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
    vehicle."Type",
    vehicle."LastModifiedAt"
FROM vehicles."Vehicles" AS vehicle
LEFT JOIN vehicles."VehicleUsers" AS vehicle_user
    ON vehicle_user."VehicleId" = vehicle."Id";

COMMIT;
