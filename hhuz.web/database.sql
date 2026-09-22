CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "Categories" (
    "Id" text NOT NULL,
    "Name" character varying(100) NOT NULL,
    "Description" text NOT NULL,
    CONSTRAINT "PK_Categories" PRIMARY KEY ("Id")
);

CREATE TABLE "Positions" (
    "Id" text NOT NULL,
    "Title" text NOT NULL,
    "ShortDescription" text NOT NULL,
    "MaxProjects" integer NOT NULL,
    "Version" integer NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Positions" PRIMARY KEY ("Id")
);

CREATE TABLE "Tags" (
    "Id" text NOT NULL,
    "Name" character varying(100) NOT NULL,
    CONSTRAINT "PK_Tags" PRIMARY KEY ("Id")
);

CREATE TABLE "Users" (
    "Id" text NOT NULL,
    username text NOT NULL,
    "Password" text NOT NULL,
    "Email" character varying(255) NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "Role" varchar(50) NOT NULL,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);

CREATE TABLE "Attributes" (
    "Id" text NOT NULL,
    "Name" character varying(150) NOT NULL,
    "Description" text NOT NULL,
    "DataType" integer NOT NULL,
    "CategoryId" text NOT NULL,
    CONSTRAINT "PK_Attributes" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Attributes_Categories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AccessRules" (
    "Id" text NOT NULL,
    "RuleType" text NOT NULL,
    "RuleName" text NOT NULL,
    "PositionId" text NOT NULL,
    CONSTRAINT "PK_AccessRules" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AccessRules_Positions_PositionId" FOREIGN KEY ("PositionId") REFERENCES "Positions" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "PositionProjectTags" (
    "Id" text NOT NULL,
    "PositionId" text NOT NULL,
    "TagId" text NOT NULL,
    CONSTRAINT "PK_PositionProjectTags" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_PositionProjectTags_Positions_PositionId" FOREIGN KEY ("PositionId") REFERENCES "Positions" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_PositionProjectTags_Tags_TagId" FOREIGN KEY ("TagId") REFERENCES "Tags" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Cvs" (
    "Id" text NOT NULL,
    "Status" integer NOT NULL,
    "Version" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UserId" text NOT NULL,
    "PositionId" text NOT NULL,
    CONSTRAINT "PK_Cvs" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Cvs_Positions_PositionId" FOREIGN KEY ("PositionId") REFERENCES "Positions" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Cvs_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "DiscussionPosts" (
    "Id" text NOT NULL,
    "Title" text NOT NULL,
    "Content" text NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UserId" text NOT NULL,
    "PositionId" text NOT NULL,
    CONSTRAINT "PK_DiscussionPosts" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_DiscussionPosts_Positions_PositionId" FOREIGN KEY ("PositionId") REFERENCES "Positions" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_DiscussionPosts_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ExternalLogins" (
    "Id" text NOT NULL,
    "LoginProvider" text NOT NULL,
    "Name" text NOT NULL,
    "UserId" text NOT NULL,
    CONSTRAINT "PK_ExternalLogins" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ExternalLogins_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Profiles" (
    "Id" text NOT NULL,
    "FirstName" text NOT NULL,
    "LastName" text NOT NULL,
    "Location" text NOT NULL,
    "PhotoUrl" text,
    "UserId" text NOT NULL,
    CONSTRAINT "PK_Profiles" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Profiles_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Projects" (
    "Id" text NOT NULL,
    "Name" text NOT NULL,
    "Description" text NOT NULL,
    "StartDate" timestamp with time zone NOT NULL,
    "EndDate" timestamp with time zone,
    "UserId" text NOT NULL,
    CONSTRAINT "PK_Projects" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Projects_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AttributeOptions" (
    "Id" text NOT NULL,
    "Value" text NOT NULL,
    "AttributeId" text NOT NULL,
    CONSTRAINT "PK_AttributeOptions" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AttributeOptions_Attributes_AttributeId" FOREIGN KEY ("AttributeId") REFERENCES "Attributes" ("Id") ON DELETE CASCADE
);

CREATE TABLE "PositionAttributes" (
    "Id" text NOT NULL,
    "Name" text NOT NULL,
    "Description" text NOT NULL,
    "IsRequired" boolean NOT NULL,
    "SortOrder" integer NOT NULL,
    "PositionId" text NOT NULL,
    "AttributeId" text NOT NULL,
    CONSTRAINT "PK_PositionAttributes" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_PositionAttributes_Attributes_AttributeId" FOREIGN KEY ("AttributeId") REFERENCES "Attributes" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_PositionAttributes_Positions_PositionId" FOREIGN KEY ("PositionId") REFERENCES "Positions" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "Likes" (
    "Id" text NOT NULL,
    "UserId" text NOT NULL,
    "CvId" text NOT NULL,
    CONSTRAINT "PK_Likes" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Likes_Cvs_CvId" FOREIGN KEY ("CvId") REFERENCES "Cvs" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Likes_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ProjectTags" (
    "Id" text NOT NULL,
    "ProjectId" text NOT NULL,
    "TagId" text NOT NULL,
    CONSTRAINT "PK_ProjectTags" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ProjectTags_Projects_ProjectId" FOREIGN KEY ("ProjectId") REFERENCES "Projects" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_ProjectTags_Tags_TagId" FOREIGN KEY ("TagId") REFERENCES "Tags" ("Id") ON DELETE CASCADE
);

CREATE TABLE "CandidateAttributeValues" (
    "Id" text NOT NULL,
    "Value" text,
    "UserId" text NOT NULL,
    "AttributeId" text NOT NULL,
    "AttributeOptionId" text,
    CONSTRAINT "PK_CandidateAttributeValues" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_CandidateAttributeValues_AttributeOptions_AttributeOptionId" FOREIGN KEY ("AttributeOptionId") REFERENCES "AttributeOptions" ("Id") ON DELETE SET NULL,
    CONSTRAINT "FK_CandidateAttributeValues_Attributes_AttributeId" FOREIGN KEY ("AttributeId") REFERENCES "Attributes" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_CandidateAttributeValues_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_AccessRules_PositionId" ON "AccessRules" ("PositionId");

CREATE UNIQUE INDEX "IX_AttributeOptions_AttributeId_Value" ON "AttributeOptions" ("AttributeId", "Value");

CREATE INDEX "IX_Attributes_CategoryId" ON "Attributes" ("CategoryId");

CREATE UNIQUE INDEX "IX_Attributes_Name" ON "Attributes" ("Name");

CREATE INDEX "IX_CandidateAttributeValues_AttributeId" ON "CandidateAttributeValues" ("AttributeId");

CREATE INDEX "IX_CandidateAttributeValues_AttributeOptionId" ON "CandidateAttributeValues" ("AttributeOptionId");

CREATE UNIQUE INDEX "IX_CandidateAttributeValues_UserId_AttributeId" ON "CandidateAttributeValues" ("UserId", "AttributeId");

CREATE UNIQUE INDEX "IX_Categories_Name" ON "Categories" ("Name");

CREATE INDEX "IX_Cvs_PositionId" ON "Cvs" ("PositionId");

CREATE INDEX "IX_Cvs_UserId" ON "Cvs" ("UserId");

CREATE INDEX "IX_DiscussionPosts_PositionId" ON "DiscussionPosts" ("PositionId");

CREATE INDEX "IX_DiscussionPosts_UserId" ON "DiscussionPosts" ("UserId");

CREATE UNIQUE INDEX "IX_ExternalLogins_LoginProvider_Name" ON "ExternalLogins" ("LoginProvider", "Name");

CREATE INDEX "IX_ExternalLogins_UserId" ON "ExternalLogins" ("UserId");

CREATE INDEX "IX_Likes_CvId" ON "Likes" ("CvId");

CREATE UNIQUE INDEX "IX_Likes_UserId_CvId" ON "Likes" ("UserId", "CvId");

CREATE INDEX "IX_PositionAttributes_AttributeId" ON "PositionAttributes" ("AttributeId");

CREATE UNIQUE INDEX "IX_PositionAttributes_PositionId_AttributeId" ON "PositionAttributes" ("PositionId", "AttributeId");

CREATE UNIQUE INDEX "IX_PositionProjectTags_PositionId_TagId" ON "PositionProjectTags" ("PositionId", "TagId");

CREATE INDEX "IX_PositionProjectTags_TagId" ON "PositionProjectTags" ("TagId");

CREATE UNIQUE INDEX "IX_Profiles_UserId" ON "Profiles" ("UserId");

CREATE INDEX "IX_Projects_UserId" ON "Projects" ("UserId");

CREATE UNIQUE INDEX "IX_ProjectTags_ProjectId_TagId" ON "ProjectTags" ("ProjectId", "TagId");

CREATE INDEX "IX_ProjectTags_TagId" ON "ProjectTags" ("TagId");

CREATE UNIQUE INDEX "IX_Tags_Name" ON "Tags" ("Name");

CREATE UNIQUE INDEX "IX_Users_Email" ON "Users" ("Email");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260922100528_InitialCreate', '8.0.28');

COMMIT;

