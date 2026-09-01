-- Dev seed data for manual poking of GET /api/tasks (filter + sort_by)
-- 15 rows, deliberately varied:
--   Completed: 5 true / 10 false
--   DueDate:   3 NULL, 4 overdue (as of 2026-08-28), 4 within ~2 weeks, 4 one-to-three months out
--   CreatedDate: spread across ~2026-08-08..28, intentionally NOT correlated with DueDate
--                so ?sort_by=createdDate and ?sort_by=dueDate give visibly different orders
--   One long TaskDescription (no length cap in the schema)
--
-- Id has no DB default (ValueGeneratedNever) -> gen_random_uuid() supplies it here
-- Requires the Tasks table to already exist (the api service creates it on startup)
--
-- Usage:
--   docker compose --profile seed up
--   docker exec -i tinyhome-postgres-manual psql -U postgres -d tinyhome < scripts/seed-dev-data.sql

TRUNCATE "Tasks";

INSERT INTO "Tasks" ("Id","TaskDescription","Completed","DueDate","CreatedDate") VALUES
(gen_random_uuid(), 'Submit quarterly tax estimate to accountant', false, '2026-08-18 00:00:00+00', '2026-08-26 14:30:00+00'),
(gen_random_uuid(), 'Replace furnace air filter', true, '2026-08-20 00:00:00+00', '2026-08-10 08:05:00+00'),
(gen_random_uuid(), 'Reply to the landlord about the lease renewal and ask whether the rent increase is negotiable given the maintenance backlog from last winter', false, '2026-08-25 00:00:00+00', '2026-08-27 19:45:00+00'),
(gen_random_uuid(), 'Return library books', false, '2026-08-27 00:00:00+00', '2026-08-14 11:20:00+00'),
(gen_random_uuid(), 'Pick up dry cleaning', true, '2026-08-30 00:00:00+00', '2026-08-24 07:50:00+00'),
(gen_random_uuid(), 'Prepare slides for Monday standup', false, '2026-09-02 00:00:00+00', '2026-08-08 16:10:00+00'),
(gen_random_uuid(), 'Schedule car service appointment', false, '2026-09-07 00:00:00+00', '2026-08-21 13:00:00+00'),
(gen_random_uuid(), 'Book dentist cleaning', false, '2026-09-11 00:00:00+00', '2026-08-19 09:35:00+00'),
(gen_random_uuid(), 'Renew passport before it expires', false, '2026-09-28 00:00:00+00', '2026-08-12 21:15:00+00'),
(gen_random_uuid(), 'Plan parents anniversary dinner and send invitations', false, '2026-10-15 00:00:00+00', '2026-08-28 06:25:00+00'),
(gen_random_uuid(), 'Winterize the garden and store the hoses', false, '2026-11-10 00:00:00+00', '2026-08-16 17:40:00+00'),
(gen_random_uuid(), 'Draft year-end donation letter', true, '2026-11-25 00:00:00+00', '2026-08-09 10:55:00+00'),
(gen_random_uuid(), 'Back up laptop to external drive', true, NULL, '2026-08-23 22:05:00+00'),
(gen_random_uuid(), 'Clean out the garage', false, NULL, '2026-08-11 12:45:00+00'),
(gen_random_uuid(), 'Read Designing Data-Intensive Applications chapter 5', true, NULL, '2026-08-25 15:30:00+00');
