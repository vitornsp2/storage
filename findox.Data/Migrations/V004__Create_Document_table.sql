CREATE TABLE IF NOT EXISTS storage.documents  (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    filename TEXT NOT NULL,
    content_type TEXT NOT NULL,
    description TEXT NULL,
    category TEXT,
    created_date TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    user_id BIGINT NOT NULL,
    CONSTRAINT fk_user_id_document FOREIGN KEY (user_id) REFERENCES storage.users (id) ON DELETE NO ACTION
);

CREATE INDEX user_id_index ON storage.documents (user_id);

CREATE TABLE IF NOT EXISTS storage.document_content (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    document_id BIGINT NOT NULL,
    data BYTEA NOT NULL,
    created_date TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_document_of_content FOREIGN KEY (document_id) REFERENCES storage.documents (id) ON DELETE NO ACTION
);