BEGIN;

CREATE OR REPLACE VIEW traffic_fines."FineReadModel" AS
SELECT
    fine."Id",
    fine."FinedUserId",
    fine."VehicleId",
    fine."Amount",
    fine."Currency",
    fine."ViolationCode",
    fine."Reason",
    fine."FineDate",
    fine."Status",
    fine."CurrentAction"
FROM traffic_fines."Fines" AS fine;

COMMIT;
