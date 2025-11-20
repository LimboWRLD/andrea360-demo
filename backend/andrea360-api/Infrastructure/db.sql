--
-- PostgreSQL database dump
--

-- Dumped from database version 17.4
-- Dumped by pg_dump version 17.4

-- Started on 2025-11-16 00:52:53

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';
SET default_table_access_method = heap;

--
-- Tables
--

CREATE TABLE public.addresses (
    id uuid NOT NULL,
    is_deleted boolean DEFAULT false NOT NULL,
    street character varying(200) NOT NULL,
    number character varying(200) NOT NULL,
    city_id uuid NOT NULL
);

ALTER TABLE public.addresses OWNER TO postgres;

CREATE TABLE public.cities (
    id uuid NOT NULL,
    is_deleted boolean DEFAULT false NOT NULL,
    name character varying(200) NOT NULL,
    country_id uuid NOT NULL
);

ALTER TABLE public.cities OWNER TO postgres;

CREATE TABLE public.countries (
    id uuid NOT NULL,
    is_deleted boolean DEFAULT false NOT NULL,
    name character varying(200) NOT NULL
);

ALTER TABLE public.countries OWNER TO postgres;

CREATE TABLE public.locations (
    id uuid NOT NULL,
    is_deleted boolean DEFAULT false NOT NULL,
    name character varying(200) NOT NULL,
    address_id uuid NOT NULL
);

ALTER TABLE public.locations OWNER TO postgres;

CREATE TABLE public.reservations (
    id uuid NOT NULL,
    is_deleted boolean DEFAULT false NOT NULL,
    session_id uuid NOT NULL,
    user_id uuid NOT NULL,
    reserved_at timestamp without time zone NOT NULL,
    is_cancelled boolean DEFAULT false NOT NULL
);

ALTER TABLE public.reservations OWNER TO postgres;

CREATE TABLE public.services (
    id uuid NOT NULL,
    is_deleted boolean DEFAULT false NOT NULL,
    name character varying(200) NOT NULL,
    price numeric(18,2) NOT NULL
);

ALTER TABLE public.services OWNER TO postgres;

CREATE TABLE public.sessions (
    id uuid NOT NULL,
    is_deleted boolean DEFAULT false NOT NULL,
    start_time timestamp without time zone NOT NULL,
    end_time timestamp without time zone NOT NULL,
    location_id uuid NOT NULL,
    service_id uuid NOT NULL,
    max_capacity integer NOT NULL,
    current_capacity integer NOT NULL
);

ALTER TABLE public.sessions OWNER TO postgres;

CREATE TABLE public.transactions (
    id uuid NOT NULL,
    is_deleted boolean DEFAULT false NOT NULL,
    user_id uuid NOT NULL,
    service_id uuid NOT NULL,
    amount numeric(18,2) NOT NULL,
    transaction_date timestamp without time zone NOT NULL,
    stripe_transaction_id character varying(255)
);

ALTER TABLE public.transactions OWNER TO postgres;

CREATE TABLE public.user_services (
    id uuid NOT NULL,
    is_deleted boolean DEFAULT false NOT NULL,
    user_id uuid NOT NULL,
    service_id uuid NOT NULL,
    remaining_sessions integer NOT NULL
);

ALTER TABLE public.user_services OWNER TO postgres;

CREATE TABLE public.users (
    id uuid NOT NULL,
    is_deleted boolean DEFAULT false NOT NULL,
    first_name character varying(100) NOT NULL,
    last_name character varying(100) NOT NULL,
    email character varying(200) NOT NULL,
    location_id uuid NOT NULL,
    stripe_customer_id character varying(200),
    keycloak_id character varying(200) NOT NULL
);

ALTER TABLE public.users OWNER TO postgres;

--
-- COPY data
--

COPY public.addresses (id, is_deleted, street, number, city_id) FROM stdin;
\.

COPY public.cities (id, is_deleted, name, country_id) FROM stdin;
\.

COPY public.countries (id, is_deleted, name) FROM stdin;
\.

COPY public.locations (id, is_deleted, name, address_id) FROM stdin;
\.

COPY public.reservations (id, is_deleted, session_id, user_id, reserved_at, is_cancelled) FROM stdin;
\.

COPY public.services (id, is_deleted, name, price) FROM stdin;
\.

COPY public.sessions (id, is_deleted, start_time, end_time, location_id, service_id, max_capacity, current_capacity) FROM stdin;
\.

COPY public.transactions (id, is_deleted, user_id, service_id, amount, transaction_date, stripe_transaction_id) FROM stdin;
\.

COPY public.user_services (id, is_deleted, user_id, service_id, remaining_sessions) FROM stdin;
\.

COPY public.users (id, is_deleted, first_name, last_name, email, location_id, stripe_customer_id, keycloak_id) FROM stdin;
\.

--
-- Primary Keys
--

ALTER TABLE ONLY public.addresses ADD CONSTRAINT addresses_pkey PRIMARY KEY (id);
ALTER TABLE ONLY public.cities ADD CONSTRAINT cities_pkey PRIMARY KEY (id);
ALTER TABLE ONLY public.countries ADD CONSTRAINT countries_pkey PRIMARY KEY (id);
ALTER TABLE ONLY public.locations ADD CONSTRAINT locations_pkey PRIMARY KEY (id);
ALTER TABLE ONLY public.reservations ADD CONSTRAINT reservations_pkey PRIMARY KEY (id);
ALTER TABLE ONLY public.services ADD CONSTRAINT services_pkey PRIMARY KEY (id);
ALTER TABLE ONLY public.sessions ADD CONSTRAINT sessions_pkey PRIMARY KEY (id);
ALTER TABLE ONLY public.transactions ADD CONSTRAINT transactions_pkey PRIMARY KEY (id);
ALTER TABLE ONLY public.user_services ADD CONSTRAINT user_services_pkey PRIMARY KEY (id);
ALTER TABLE ONLY public.users ADD CONSTRAINT users_pkey PRIMARY KEY (id);

--
-- Indexes
--

CREATE UNIQUE INDEX ix_cities_name ON public.cities USING btree (name);
CREATE UNIQUE INDEX ix_countries_name ON public.countries USING btree (name);
CREATE UNIQUE INDEX ix_services_name ON public.services USING btree (name);
CREATE INDEX ix_sessions_location_id ON public.sessions USING btree (location_id);
CREATE INDEX ix_sessions_service_id ON public.sessions USING btree (service_id);
CREATE INDEX ix_transactions_service_id ON public.transactions USING btree (service_id);
CREATE INDEX ix_transactions_transaction_date ON public.transactions USING btree (transaction_date);
CREATE INDEX ix_transactions_user_id ON public.transactions USING btree (user_id);
CREATE INDEX ix_user_services_service_id ON public.user_services USING btree (service_id);
CREATE INDEX ix_user_services_user_id ON public.user_services USING btree (user_id);
CREATE UNIQUE INDEX ix_user_services_user_id_service_id ON public.user_services USING btree (user_id, service_id);
CREATE UNIQUE INDEX ix_users_email ON public.users USING btree (email);

--
-- Foreign Keys
--

ALTER TABLE ONLY public.addresses ADD CONSTRAINT fk_addresses_cities_city_id FOREIGN KEY (city_id) REFERENCES public.cities(id) ON DELETE CASCADE;
ALTER TABLE ONLY public.cities ADD CONSTRAINT fk_cities_countries_country_id FOREIGN KEY (country_id) REFERENCES public.countries(id) ON DELETE CASCADE;
ALTER TABLE ONLY public.locations ADD CONSTRAINT fk_locations_addresses_address_id FOREIGN KEY (address_id) REFERENCES public.addresses(id) ON DELETE CASCADE;
ALTER TABLE ONLY public.reservations ADD CONSTRAINT fk_reservations_sessions_session_id FOREIGN KEY (session_id) REFERENCES public.sessions(id) ON DELETE CASCADE;
ALTER TABLE ONLY public.reservations ADD CONSTRAINT fk_reservations_users_user_id FOREIGN KEY (user_id) REFERENCES public.users(id) ON DELETE CASCADE;
ALTER TABLE ONLY public.sessions ADD CONSTRAINT fk_sessions_locations_location_id FOREIGN KEY (location_id) REFERENCES public.locations(id) ON DELETE CASCADE;
ALTER TABLE ONLY public.sessions ADD CONSTRAINT fk_sessions_services_service_id FOREIGN KEY (service_id) REFERENCES public.services(id) ON DELETE CASCADE;
ALTER TABLE ONLY public.transactions ADD CONSTRAINT fk_transactions_services_service_id FOREIGN KEY (service_id) REFERENCES public.services(id) ON DELETE CASCADE;
ALTER TABLE ONLY public.transactions ADD CONSTRAINT fk_transactions_users_user_id FOREIGN KEY (user_id) REFERENCES public.users(id) ON DELETE CASCADE;
ALTER TABLE ONLY public.user_services ADD CONSTRAINT fk_user_services_services_service_id FOREIGN KEY (service_id) REFERENCES public.services(id) ON DELETE CASCADE;
ALTER TABLE ONLY public.user_services ADD CONSTRAINT fk_user_services_users_user_id FOREIGN KEY (user_id) REFERENCES public.users(id) ON DELETE CASCADE;
ALTER TABLE ONLY public.users ADD CONSTRAINT fk_users_locations_location_id FOREIGN KEY (location_id) REFERENCES public.locations(id) ON DELETE CASCADE;
