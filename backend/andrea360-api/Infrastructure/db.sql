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
-- TOC entry 219 (class 1259 OID 22491)
-- Name: Addresses; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Addresses" (
    "Id" uuid NOT NULL,
    "IsDeleted" boolean DEFAULT false NOT NULL,
    "Street" character varying(200) NOT NULL,
    "Number" character varying(200) NOT NULL,
    "CityId" uuid NOT NULL
);


ALTER TABLE public."Addresses" OWNER TO postgres;

--
-- TOC entry 218 (class 1259 OID 22479)
-- Name: Cities; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Cities" (
    "Id" uuid NOT NULL,
    "IsDeleted" boolean DEFAULT false NOT NULL,
    "Name" character varying(200) NOT NULL,
    "CountryId" uuid NOT NULL
);


ALTER TABLE public."Cities" OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 22472)
-- Name: Countries; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Countries" (
    "Id" uuid NOT NULL,
    "IsDeleted" boolean DEFAULT false NOT NULL,
    "Name" character varying(200) NOT NULL
);


ALTER TABLE public."Countries" OWNER TO postgres;

--
-- TOC entry 220 (class 1259 OID 22502)
-- Name: Locations; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Locations" (
    "Id" uuid NOT NULL,
    "IsDeleted" boolean DEFAULT false NOT NULL,
    "Name" character varying(200) NOT NULL,
    "AddressId" uuid NOT NULL
);


ALTER TABLE public."Locations" OWNER TO postgres;

--
-- TOC entry 224 (class 1259 OID 22552)
-- Name: Reservations; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Reservations" (
    "Id" uuid NOT NULL,
    "IsDeleted" boolean DEFAULT false NOT NULL,
    "SessionId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "ReservedAt" timestamp without time zone NOT NULL,
    "IsCancelled" boolean DEFAULT false NOT NULL
);


ALTER TABLE public."Reservations" OWNER TO postgres;

--
-- TOC entry 221 (class 1259 OID 22513)
-- Name: Services; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Services" (
    "Id" uuid NOT NULL,
    "IsDeleted" boolean DEFAULT false NOT NULL,
    "Name" character varying(200) NOT NULL,
    "Price" numeric(18,2) NOT NULL
);


ALTER TABLE public."Services" OWNER TO postgres;

--
-- TOC entry 223 (class 1259 OID 22534)
-- Name: Sessions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Sessions" (
    "Id" uuid NOT NULL,
    "IsDeleted" boolean DEFAULT false NOT NULL,
    "StartTime" timestamp without time zone NOT NULL,
    "EndTime" timestamp without time zone NOT NULL,
    "LocationId" uuid NOT NULL,
    "ServiceId" uuid NOT NULL,
    "MaxCapacity" integer NOT NULL,
    "CurrentCapacity" integer NOT NULL
);


ALTER TABLE public."Sessions" OWNER TO postgres;

--
-- TOC entry 226 (class 1259 OID 22588)
-- Name: Transactions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Transactions" (
    "Id" uuid NOT NULL,
    "IsDeleted" boolean DEFAULT false NOT NULL,
    "UserId" uuid NOT NULL,
    "ServiceId" uuid NOT NULL,
    "Amount" numeric(18,2) NOT NULL,
    "TransactionDate" timestamp without time zone NOT NULL,
    "StripeTransactionId" character varying(255)
);


ALTER TABLE public."Transactions" OWNER TO postgres;

--
-- TOC entry 225 (class 1259 OID 22569)
-- Name: UserServices; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."UserServices" (
    "Id" uuid NOT NULL,
    "IsDeleted" boolean DEFAULT false NOT NULL,
    "UserId" uuid NOT NULL,
    "ServiceId" uuid NOT NULL,
    "RemainingSessions" integer NOT NULL
);


ALTER TABLE public."UserServices" OWNER TO postgres;

--
-- TOC entry 222 (class 1259 OID 22520)
-- Name: Users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Users" (
    "Id" uuid NOT NULL,
    "IsDeleted" boolean DEFAULT false NOT NULL,
    "FirstName" character varying(100) NOT NULL,
    "LastName" character varying(100) NOT NULL,
    "Email" character varying(200) NOT NULL,
    "LocationId" uuid NOT NULL,
    "StripeCustomerId" character varying(200),
    "KeycloakId" character varying(200) NOT NULL
);


ALTER TABLE public."Users" OWNER TO postgres;

--
-- TOC entry 4979 (class 0 OID 22491)
-- Dependencies: 219
-- Data for Name: Addresses; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Addresses" ("Id", "IsDeleted", "Street", "Number", "CityId") FROM stdin;
\.


--
-- TOC entry 4978 (class 0 OID 22479)
-- Dependencies: 218
-- Data for Name: Cities; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Cities" ("Id", "IsDeleted", "Name", "CountryId") FROM stdin;
\.


--
-- TOC entry 4977 (class 0 OID 22472)
-- Dependencies: 217
-- Data for Name: Countries; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Countries" ("Id", "IsDeleted", "Name") FROM stdin;
\.


--
-- TOC entry 4980 (class 0 OID 22502)
-- Dependencies: 220
-- Data for Name: Locations; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Locations" ("Id", "IsDeleted", "Name", "AddressId") FROM stdin;
\.


--
-- TOC entry 4984 (class 0 OID 22552)
-- Dependencies: 224
-- Data for Name: Reservations; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Reservations" ("Id", "IsDeleted", "SessionId", "UserId", "ReservedAt", "IsCancelled") FROM stdin;
\.


--
-- TOC entry 4981 (class 0 OID 22513)
-- Dependencies: 221
-- Data for Name: Services; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Services" ("Id", "IsDeleted", "Name", "Price") FROM stdin;
\.


--
-- TOC entry 4983 (class 0 OID 22534)
-- Dependencies: 223
-- Data for Name: Sessions; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Sessions" ("Id", "IsDeleted", "StartTime", "EndTime", "LocationId", "ServiceId", "MaxCapacity", "CurrentCapacity") FROM stdin;
\.


--
-- TOC entry 4986 (class 0 OID 22588)
-- Dependencies: 226
-- Data for Name: Transactions; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Transactions" ("Id", "IsDeleted", "UserId", "ServiceId", "Amount", "TransactionDate", "StripeTransactionId") FROM stdin;
\.


--
-- TOC entry 4985 (class 0 OID 22569)
-- Dependencies: 225
-- Data for Name: UserServices; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."UserServices" ("Id", "IsDeleted", "UserId", "ServiceId", "RemainingSessions") FROM stdin;
\.


--
-- TOC entry 4982 (class 0 OID 22520)
-- Dependencies: 222
-- Data for Name: Users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Users" ("Id", "IsDeleted", "FirstName", "LastName", "Email", "LocationId", "StripeCustomerId", "KeycloakId") FROM stdin;
\.


--
-- TOC entry 4795 (class 2606 OID 22496)
-- Name: Addresses Addresses_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Addresses"
    ADD CONSTRAINT "Addresses_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 4792 (class 2606 OID 22484)
-- Name: Cities Cities_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Cities"
    ADD CONSTRAINT "Cities_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 4789 (class 2606 OID 22477)
-- Name: Countries Countries_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Countries"
    ADD CONSTRAINT "Countries_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 4797 (class 2606 OID 22507)
-- Name: Locations Locations_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Locations"
    ADD CONSTRAINT "Locations_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 4809 (class 2606 OID 22558)
-- Name: Reservations Reservations_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Reservations"
    ADD CONSTRAINT "Reservations_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 4800 (class 2606 OID 22518)
-- Name: Services Services_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Services"
    ADD CONSTRAINT "Services_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 4807 (class 2606 OID 22539)
-- Name: Sessions Sessions_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Sessions"
    ADD CONSTRAINT "Sessions_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 4819 (class 2606 OID 22593)
-- Name: Transactions Transactions_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Transactions"
    ADD CONSTRAINT "Transactions_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 4814 (class 2606 OID 22574)
-- Name: UserServices UserServices_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserServices"
    ADD CONSTRAINT "UserServices_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 4803 (class 2606 OID 22527)
-- Name: Users Users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "Users_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 4793 (class 1259 OID 22485)
-- Name: IX_Cities_Name; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_Cities_Name" ON public."Cities" USING btree ("Name");


--
-- TOC entry 4790 (class 1259 OID 22478)
-- Name: IX_Countries_Name; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_Countries_Name" ON public."Countries" USING btree ("Name");


--
-- TOC entry 4798 (class 1259 OID 22519)
-- Name: IX_Services_Name; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_Services_Name" ON public."Services" USING btree ("Name");


--
-- TOC entry 4804 (class 1259 OID 22550)
-- Name: IX_Sessions_LocationId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Sessions_LocationId" ON public."Sessions" USING btree ("LocationId");


--
-- TOC entry 4805 (class 1259 OID 22551)
-- Name: IX_Sessions_ServiceId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Sessions_ServiceId" ON public."Sessions" USING btree ("ServiceId");


--
-- TOC entry 4815 (class 1259 OID 22595)
-- Name: IX_Transactions_ServiceId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Transactions_ServiceId" ON public."Transactions" USING btree ("ServiceId");


--
-- TOC entry 4816 (class 1259 OID 22596)
-- Name: IX_Transactions_TransactionDate; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Transactions_TransactionDate" ON public."Transactions" USING btree ("TransactionDate");


--
-- TOC entry 4817 (class 1259 OID 22594)
-- Name: IX_Transactions_UserId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Transactions_UserId" ON public."Transactions" USING btree ("UserId");


--
-- TOC entry 4810 (class 1259 OID 22576)
-- Name: IX_UserServices_ServiceId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_UserServices_ServiceId" ON public."UserServices" USING btree ("ServiceId");


--
-- TOC entry 4811 (class 1259 OID 22575)
-- Name: IX_UserServices_UserId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_UserServices_UserId" ON public."UserServices" USING btree ("UserId");


--
-- TOC entry 4812 (class 1259 OID 22577)
-- Name: IX_UserServices_UserId_ServiceId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_UserServices_UserId_ServiceId" ON public."UserServices" USING btree ("UserId", "ServiceId");


--
-- TOC entry 4801 (class 1259 OID 22528)
-- Name: IX_Users_Email; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_Users_Email" ON public."Users" USING btree ("Email");


--
-- TOC entry 4821 (class 2606 OID 22497)
-- Name: Addresses FK_Addresses_Cities_CityId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Addresses"
    ADD CONSTRAINT "FK_Addresses_Cities_CityId" FOREIGN KEY ("CityId") REFERENCES public."Cities"("Id") ON DELETE CASCADE;


--
-- TOC entry 4820 (class 2606 OID 22486)
-- Name: Cities FK_Cities_Countries_CountryId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Cities"
    ADD CONSTRAINT "FK_Cities_Countries_CountryId" FOREIGN KEY ("CountryId") REFERENCES public."Countries"("Id") ON DELETE CASCADE;


--
-- TOC entry 4822 (class 2606 OID 22508)
-- Name: Locations FK_Locations_Addresses_AddressId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Locations"
    ADD CONSTRAINT "FK_Locations_Addresses_AddressId" FOREIGN KEY ("AddressId") REFERENCES public."Addresses"("Id") ON DELETE CASCADE;


--
-- TOC entry 4826 (class 2606 OID 22559)
-- Name: Reservations FK_Reservations_Sessions_SessionId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Reservations"
    ADD CONSTRAINT "FK_Reservations_Sessions_SessionId" FOREIGN KEY ("SessionId") REFERENCES public."Sessions"("Id") ON DELETE CASCADE;


--
-- TOC entry 4827 (class 2606 OID 22564)
-- Name: Reservations FK_Reservations_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Reservations"
    ADD CONSTRAINT "FK_Reservations_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- TOC entry 4824 (class 2606 OID 22540)
-- Name: Sessions FK_Sessions_Locations_LocationId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Sessions"
    ADD CONSTRAINT "FK_Sessions_Locations_LocationId" FOREIGN KEY ("LocationId") REFERENCES public."Locations"("Id") ON DELETE CASCADE;


--
-- TOC entry 4825 (class 2606 OID 22545)
-- Name: Sessions FK_Sessions_Services_ServiceId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Sessions"
    ADD CONSTRAINT "FK_Sessions_Services_ServiceId" FOREIGN KEY ("ServiceId") REFERENCES public."Services"("Id") ON DELETE CASCADE;


--
-- TOC entry 4830 (class 2606 OID 22602)
-- Name: Transactions FK_Transactions_Services_ServiceId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Transactions"
    ADD CONSTRAINT "FK_Transactions_Services_ServiceId" FOREIGN KEY ("ServiceId") REFERENCES public."Services"("Id") ON DELETE CASCADE;


--
-- TOC entry 4831 (class 2606 OID 22597)
-- Name: Transactions FK_Transactions_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Transactions"
    ADD CONSTRAINT "FK_Transactions_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- TOC entry 4828 (class 2606 OID 22583)
-- Name: UserServices FK_UserServices_Services_ServiceId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserServices"
    ADD CONSTRAINT "FK_UserServices_Services_ServiceId" FOREIGN KEY ("ServiceId") REFERENCES public."Services"("Id") ON DELETE CASCADE;


--
-- TOC entry 4829 (class 2606 OID 22578)
-- Name: UserServices FK_UserServices_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserServices"
    ADD CONSTRAINT "FK_UserServices_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- TOC entry 4823 (class 2606 OID 22529)
-- Name: Users FK_Users_Locations_LocationId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "FK_Users_Locations_LocationId" FOREIGN KEY ("LocationId") REFERENCES public."Locations"("Id") ON DELETE CASCADE;


-- Completed on 2025-11-16 00:52:53

--
-- PostgreSQL database dump complete
--

