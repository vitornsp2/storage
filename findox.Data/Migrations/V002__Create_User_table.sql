
CREATE TABLE IF NOT EXISTS storage.users  (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name VARCHAR(100)  NOT NULL,
    password TEXT NOT NULL,
    email TEXT UNIQUE NOT NULL,
    role  TEXT NOT NULL CHECK (role IN ('regular', 'manager', 'admin')) DEFAULT 'regular',
    created_date TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX user_role_index ON storage.users (role);