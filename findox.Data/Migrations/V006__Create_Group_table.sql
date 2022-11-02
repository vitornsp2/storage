CREATE TABLE IF NOT EXISTS storage.groups (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name TEXT NOT NULL,
    description TEXT NULL,
    created_date TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS storage.users_groups (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    group_id BIGINT NOT NULL,
    user_id BIGINT NOT NULL,
    created_date TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_group_of_users FOREIGN KEY (group_id) REFERENCES storage.groups (id) ON DELETE NO ACTION,
    CONSTRAINT fk_user_of_groups FOREIGN KEY (user_id) REFERENCES storage.users (id) ON DELETE NO ACTION
);

CREATE INDEX group_users_index ON storage.users_groups (group_id);
CREATE INDEX user_groups_index ON storage.users_groups (user_id);