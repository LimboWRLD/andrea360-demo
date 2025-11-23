-- =============================================
-- 1. ČIŠĆENJE BAZE (Opciono - briše stare podatke)
-- =============================================
TRUNCATE TABLE 
    public.reservations, 
    public.transactions, 
    public.user_services, 
    public.sessions, 
    public.users, 
    public.locations, 
    public.services, 
    public.addresses, 
    public.cities, 
    public.countries 
CASCADE;

-- =============================================
-- 2. GEOGRAFIJA I LOKACIJE
-- =============================================

-- Država
INSERT INTO public.countries (id, is_deleted, name) VALUES
('11111111-1111-1111-1111-111111111111', false, 'Srbija');

-- Gradovi
INSERT INTO public.cities (id, is_deleted, name, country_id) VALUES
('22222222-2222-2222-2222-111111111111', false, 'Beograd', '11111111-1111-1111-1111-111111111111'),
('22222222-2222-2222-2222-222222222222', false, 'Novi Sad', '11111111-1111-1111-1111-111111111111');

-- Adrese
INSERT INTO public.addresses (id, is_deleted, street, number, city_id) VALUES
('33333333-3333-3333-3333-111111111111', false, 'Krunska', '50', '22222222-2222-2222-2222-111111111111'),
('33333333-3333-3333-3333-222222222222', false, 'Zmaj Jovina', '10', '22222222-2222-2222-2222-222222222222');

-- Lokacije
INSERT INTO public.locations (id, is_deleted, name, address_id) VALUES
('44444444-4444-4444-4444-111111111111', false, 'Andrea360 Vračar', '33333333-3333-3333-3333-111111111111'),
('44444444-4444-4444-4444-222222222222', false, 'Andrea360 Novi Sad', '33333333-3333-3333-3333-222222222222');

-- =============================================
-- 3. USLUGE (SERVICES)
-- =============================================
INSERT INTO public.services (id, is_deleted, name, price) VALUES
('55555555-5555-5555-5555-111111111111', false, 'CrossFit', 30.00),
('55555555-5555-5555-5555-222222222222', false, 'Yoga', 20.00),
('55555555-5555-5555-5555-333333333333', false, 'Pilates', 25.00);

-- =============================================
-- 4. KORISNICI (USERS)
-- NAPOMENA: keycloak_id ovde mora da se poklapa sa onim što imate u Keycloak-u.
-- Ovde su stavljeni generički UUID-ovi, ako imate prave Keycloak usere, zamenite ih.
-- =============================================

-- ADMIN (Vezan za Vračar, ali ima pristup svemu)
INSERT INTO public.users (id, is_deleted, first_name, last_name, email, location_id, stripe_customer_id, keycloak_id, roles) VALUES
('66666666-6666-6666-6666-000000000001', false, 'Admin', 'Adminovic', 'admin@andrea360.com', '44444444-4444-4444-4444-111111111111', NULL, 'kc-admin-uuid-1234', ARRAY['admin']);

-- ZAPOSLENI 1 (Beograd)
INSERT INTO public.users (id, is_deleted, first_name, last_name, email, location_id, stripe_customer_id, keycloak_id, roles) VALUES
('66666666-6666-6666-6666-000000000002', false, 'Marko', 'Markovic', 'marko@andrea360.com', '44444444-4444-4444-4444-111111111111', NULL, 'kc-employee1-uuid-1234', ARRAY['employee']);

-- ZAPOSLENI 2 (Novi Sad)
INSERT INTO public.users (id, is_deleted, first_name, last_name, email, location_id, stripe_customer_id, keycloak_id, roles) VALUES
('66666666-6666-6666-6666-000000000003', false, 'Jovana', 'Jovanovic', 'jovana@andrea360.com', '44444444-4444-4444-4444-222222222222', NULL, 'kc-employee2-uuid-1234', ARRAY['employee']);

-- ČLAN 1 (Beograd - Ima Stripe ID)
INSERT INTO public.users (id, is_deleted, first_name, last_name, email, location_id, stripe_customer_id, keycloak_id, roles) VALUES
('66666666-6666-6666-6666-000000000004', false, 'Petar', 'Petrovic', 'petar@gmail.com', '44444444-4444-4444-4444-111111111111', 'cus_TestStripeID123', 'kc-member1-uuid-1234', ARRAY['member']);

-- ČLAN 2 (Novi Sad)
INSERT INTO public.users (id, is_deleted, first_name, last_name, email, location_id, stripe_customer_id, keycloak_id, roles) VALUES
('66666666-6666-6666-6666-000000000005', false, 'Ana', 'Anic', 'ana@gmail.com', '44444444-4444-4444-4444-222222222222', NULL, 'kc-member2-uuid-1234', ARRAY['member']);


-- =============================================
-- 5. TERMINI (SESSIONS) - Zakazani u budućnosti
-- =============================================

-- CrossFit u Beogradu (Sutra u 18:00)
INSERT INTO public.sessions (id, is_deleted, start_time, end_time, location_id, service_id, max_capacity, current_capacity) VALUES
('77777777-7777-7777-7777-111111111111', false, NOW() + interval '1 day' + interval '18 hours', NOW() + interval '1 day' + interval '19 hours', '44444444-4444-4444-4444-111111111111', '55555555-5555-5555-5555-111111111111', 15, 1);

-- Yoga u Novom Sadu (Prekosutra u 10:00)
INSERT INTO public.sessions (id, is_deleted, start_time, end_time, location_id, service_id, max_capacity, current_capacity) VALUES
('77777777-7777-7777-7777-222222222222', false, NOW() + interval '2 days' + interval '10 hours', NOW() + interval '2 days' + interval '11 hours', '44444444-4444-4444-4444-222222222222', '55555555-5555-5555-5555-222222222222', 10, 0);

-- =============================================
-- 6. KREDITI ČLANOVA (USER_SERVICES) & TRANSAKCIJE
-- =============================================

-- Član Petar je kupio 5 CrossFit treninga pre 2 dana
INSERT INTO public.transactions (id, is_deleted, user_id, service_id, amount, transaction_date, stripe_transaction_id) VALUES
('88888888-8888-8888-8888-111111111111', false, '66666666-6666-6666-6666-000000000004', '55555555-5555-5555-5555-111111111111', 150.00, NOW() - interval '2 days', 'pi_StripePaymentIntent123');

-- Petar ima još 4 termina (jedan je rezervisao dole)
INSERT INTO public.user_services (id, is_deleted, user_id, service_id, remaining_sessions) VALUES
('99999999-9999-9999-9999-111111111111', false, '66666666-6666-6666-6666-000000000004', '55555555-5555-5555-5555-111111111111', 4);


-- =============================================
-- 7. REZERVACIJE (RESERVATIONS)
-- =============================================

-- Petar je rezervisao CrossFit u Beogradu (Session 1)
INSERT INTO public.reservations (id, is_deleted, session_id, user_id, reserved_at, is_cancelled) VALUES
('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', false, '77777777-7777-7777-7777-111111111111', '66666666-6666-6666-6666-000000000004', NOW() - interval '1 day', false);