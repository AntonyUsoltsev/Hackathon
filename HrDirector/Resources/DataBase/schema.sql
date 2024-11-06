CREATE TABLE Hackathons
(
    id             SERIAL PRIMARY KEY,
    result_harmony DOUBLE PRECISION
);

CREATE TABLE Employees
(
    id   SERIAL PRIMARY KEY,
    role VARCHAR(255),
    name VARCHAR(255)
);

CREATE TABLE Teams
(
    id      SERIAL PRIMARY KEY,
    hackathon_id INTEGER,
    junior_id    INTEGER,
    teamlead_id  INTEGER
);

CREATE TABLE Wishlists
(
    id        SERIAL PRIMARY KEY,
    hackathon_id       INTEGER,
    employee_id        INTEGER,
    chosen_employee_id INTEGER,
    rank               INTEGER
);

CREATE TABLE Satisfactions
(
    id   SERIAL PRIMARY KEY,
    hackathon_id      INTEGER,
    employee_id       INTEGER,
    satisfaction_rank INTEGER
);

ALTER TABLE Teams
    ADD CONSTRAINT fk_teams_hackathon
        FOREIGN KEY (hackathon_id) REFERENCES Hackathons (id);

ALTER TABLE Teams
    ADD CONSTRAINT fk_teams_junior
        FOREIGN KEY (junior_id) REFERENCES Employees (id);

ALTER TABLE Teams
    ADD CONSTRAINT fk_teams_teamlead
        FOREIGN KEY (teamlead_id) REFERENCES Employees (id);

ALTER TABLE Wishlists
    ADD CONSTRAINT fk_wishlist_hackathon
        FOREIGN KEY (hackathon_id) REFERENCES Hackathons (id);

ALTER TABLE Wishlists
    ADD CONSTRAINT fk_wishlist_employee
        FOREIGN KEY (employee_id) REFERENCES Employees (id);

ALTER TABLE Wishlists
    ADD CONSTRAINT fk_wishlist_chosen_employee
        FOREIGN KEY (chosen_employee_id) REFERENCES Employees (id);

ALTER TABLE Satisfactions
    ADD CONSTRAINT fk_satisfaction_hackathon
        FOREIGN KEY (hackathon_id) REFERENCES Hackathons (id);

ALTER TABLE Satisfactions
    ADD CONSTRAINT fk_satisfaction_employee
        FOREIGN KEY (employee_id) REFERENCES Employees (id);
