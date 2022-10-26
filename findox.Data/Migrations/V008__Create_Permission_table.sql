CREATE TABLE IF NOT EXISTS storage.permissions (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    document_id BIGINT NOT NULL,
    user_id BIGINT NULL,
    group_id BIGINT NULL, 
    CONSTRAINT fk_documents_of_permissions FOREIGN KEY (document_id) REFERENCES storage.documents (id) ON DELETE NO ACTION,
    CONSTRAINT fk_user_of_permissions FOREIGN KEY (user_id) REFERENCES storage.users (id) ON DELETE NO ACTION,
    CONSTRAINT fk_group_of_permissions FOREIGN KEY (group_id) REFERENCES storage.groups (id) ON DELETE NO ACTION
);