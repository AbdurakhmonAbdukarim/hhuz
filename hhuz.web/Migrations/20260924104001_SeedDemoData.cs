using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hhuz.Migrations
{
    /// <inheritdoc />
    public partial class SeedDemoData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                INSERT INTO "Categories" ("Id","Name","Description") VALUES
                ('seed-cat-certification','Certification','Professional certificates and exams'),
                ('seed-cat-domain','Domain Knowledge','Field-specific knowledge and academic performance'),
                ('seed-cat-personal','Personal Information','General personal and availability information'),
                ('seed-cat-soft','Soft Skills','Communication, language and interpersonal skills');

                INSERT INTO "Tags" ("Id","Name") VALUES
                ('seed-tag-python','Python'),
                ('seed-tag-sql','SQL'),
                ('seed-tag-react','React'),
                ('seed-tag-dataeng','Data Engineering'),
                ('seed-tag-hadoop','Apache Hadoop'),
                ('seed-tag-js','JavaScript');

                INSERT INTO "Users" ("Id","username","Password","Email","CreatedAt","Role","IsBlocked") VALUES
                ('seed-user-admin','admin','AQAAAAIAAYagAAAAEDUvnCmKSU6guXIJAfoNzFNjkgyjKWM2PCxSmYDsdiUL2evUSBUld0FYqELpDmSm1g==','admin@hhuz.com',now(),'ROLE_ADMIN',false),
                ('seed-user-recruiter1','recruiter1','AQAAAAIAAYagAAAAEJRzDWw6JynU3KR0XTW1fXHsLLpbP5WDHbYhc13vs4pssicaB0xdifNDJOhIZhGwuw==','recruiter1@hhuz.com',now(),'ROLE_RECRUITER',false),
                ('seed-user-recruiter2','recruiter2','AQAAAAIAAYagAAAAEJRzDWw6JynU3KR0XTW1fXHsLLpbP5WDHbYhc13vs4pssicaB0xdifNDJOhIZhGwuw==','recruiter2@hhuz.com',now(),'ROLE_RECRUITER',false),
                ('seed-user-candidate1','candidate1','AQAAAAIAAYagAAAAELFDbc/pgWBaGzTU1QTZQcf90g5x37MFM17DUAYR6F3eNL77b98cbOyeAc5idEgG5w==','candidate1@hhuz.com',now(),'ROLE_CANDIDATE',false),
                ('seed-user-candidate2','candidate2','AQAAAAIAAYagAAAAELFDbc/pgWBaGzTU1QTZQcf90g5x37MFM17DUAYR6F3eNL77b98cbOyeAc5idEgG5w==','candidate2@hhuz.com',now(),'ROLE_CANDIDATE',false),
                ('seed-user-candidate3','candidate3','AQAAAAIAAYagAAAAELFDbc/pgWBaGzTU1QTZQcf90g5x37MFM17DUAYR6F3eNL77b98cbOyeAc5idEgG5w==','candidate3@hhuz.com',now(),'ROLE_CANDIDATE',false);

                INSERT INTO "Attributes" ("Id","Name","Description","DataType","CategoryId","Version") VALUES
                ('seed-attr-english','English Level','Spoken and written English proficiency level',7,'seed-cat-soft',1),
                ('seed-attr-gpa','GPA','Grade point average on a 0.0 to 4.0 scale',3,'seed-cat-domain',1),
                ('seed-attr-ielts','IELTS Score','Overall IELTS band score',3,'seed-cat-certification',1),
                ('seed-attr-remote','Remote Work Availability','Whether the candidate is available for remote work',6,'seed-cat-personal',1),
                ('seed-attr-cap','CAP','Certified Analytics Professional certification level',7,'seed-cat-certification',1),
                ('seed-attr-presentation','Presentation Skills','Self-assessed presentation and public speaking level',7,'seed-cat-soft',1);

                INSERT INTO "AttributeOptions" ("Id","Value","AttributeId") VALUES
                ('seed-opt-eng-beginner','Beginner','seed-attr-english'),
                ('seed-opt-eng-intermediate','Intermediate','seed-attr-english'),
                ('seed-opt-eng-advanced','Advanced','seed-attr-english'),
                ('seed-opt-eng-native','Native','seed-attr-english'),
                ('seed-opt-cap-none','None','seed-attr-cap'),
                ('seed-opt-cap-essentials','Essentials','seed-attr-cap'),
                ('seed-opt-cap-pro','Pro','seed-attr-cap'),
                ('seed-opt-cap-expert','Expert','seed-attr-cap'),
                ('seed-opt-pres-beginner','Beginner','seed-attr-presentation'),
                ('seed-opt-pres-intermediate','Intermediate','seed-attr-presentation'),
                ('seed-opt-pres-advanced','Advanced','seed-attr-presentation');

                INSERT INTO "Positions" ("Id","Title","ShortDescription","MaxProjects","Version","IsDeleted","CreatedAt") VALUES
                ('seed-pos-ba','Business Analyst','Analyze business requirements and bridge the gap between stakeholders and engineering teams.',3,1,false,now()),
                ('seed-pos-jde','Junior Data Engineer','Entry-level role building and maintaining data pipelines with Python and SQL.',4,1,false,now());

                INSERT INTO "PositionAttributes" ("Id","Name","Description","IsRequired","SortOrder","PositionId","AttributeId") VALUES
                ('seed-posattr-ba-english','English Level','Required English proficiency',true,1,'seed-pos-ba','seed-attr-english'),
                ('seed-posattr-ba-presentation','Presentation Skills','Required presentation skill level',true,2,'seed-pos-ba','seed-attr-presentation'),
                ('seed-posattr-jde-english','English Level','Required English proficiency',true,1,'seed-pos-jde','seed-attr-english'),
                ('seed-posattr-jde-gpa','GPA','Minimum academic performance',true,2,'seed-pos-jde','seed-attr-gpa'),
                ('seed-posattr-jde-cap','CAP','Certified Analytics Professional level',false,3,'seed-pos-jde','seed-attr-cap');

                INSERT INTO "PositionProjectTags" ("Id","PositionId","TagId") VALUES
                ('seed-ppt-ba-react','seed-pos-ba','seed-tag-react'),
                ('seed-ppt-ba-js','seed-pos-ba','seed-tag-js'),
                ('seed-ppt-jde-python','seed-pos-jde','seed-tag-python'),
                ('seed-ppt-jde-sql','seed-pos-jde','seed-tag-sql'),
                ('seed-ppt-jde-dataeng','seed-pos-jde','seed-tag-dataeng');

                INSERT INTO "Profiles" ("Id","FirstName","LastName","Location","PhotoUrl","UserId","Version") VALUES
                ('seed-profile-candidate1','Malika','Yusupova','Tashkent, Uzbekistan',NULL,'seed-user-candidate1',1),
                ('seed-profile-candidate2','Javlon','Karimov','Samarkand, Uzbekistan',NULL,'seed-user-candidate2',1),
                ('seed-profile-candidate3','Dilnoza','Rashidova','Bukhara, Uzbekistan',NULL,'seed-user-candidate3',1);

                INSERT INTO "Projects" ("Id","Name","Description","StartDate","EndDate","UserId") VALUES
                ('seed-proj-c1-a','E-commerce Recommendation Engine','Built a collaborative filtering recommendation engine for an e-commerce platform using Python and Spark, improving click-through rate by 18 percent.','2024-01-01','2024-06-30','seed-user-candidate1'),
                ('seed-proj-c1-b','Stakeholder Requirements Portal','Developed an internal React dashboard to track and prioritize stakeholder requirements across departments.','2023-03-01','2023-09-30','seed-user-candidate1'),
                ('seed-proj-c2-a','Internal Analytics Dashboard','Built a SQL-backed analytics dashboard with a React front-end for the sales team.','2024-02-01',NULL,'seed-user-candidate2');

                INSERT INTO "ProjectTags" ("Id","ProjectId","TagId") VALUES
                ('seed-pt-c1a-python','seed-proj-c1-a','seed-tag-python'),
                ('seed-pt-c1a-dataeng','seed-proj-c1-a','seed-tag-dataeng'),
                ('seed-pt-c1a-sql','seed-proj-c1-a','seed-tag-sql'),
                ('seed-pt-c1b-react','seed-proj-c1-b','seed-tag-react'),
                ('seed-pt-c1b-js','seed-proj-c1-b','seed-tag-js'),
                ('seed-pt-c2a-sql','seed-proj-c2-a','seed-tag-sql'),
                ('seed-pt-c2a-react','seed-proj-c2-a','seed-tag-react');

                INSERT INTO "CandidateAttributeValues" ("Id","Value","UserId","AttributeId","AttributeOptionId") VALUES
                ('seed-cav-c1-english',NULL,'seed-user-candidate1','seed-attr-english','seed-opt-eng-advanced'),
                ('seed-cav-c1-gpa','3.8','seed-user-candidate1','seed-attr-gpa',NULL),
                ('seed-cav-c1-ielts','7.5','seed-user-candidate1','seed-attr-ielts',NULL),
                ('seed-cav-c1-remote','true','seed-user-candidate1','seed-attr-remote',NULL),
                ('seed-cav-c1-cap',NULL,'seed-user-candidate1','seed-attr-cap','seed-opt-cap-pro'),
                ('seed-cav-c1-presentation',NULL,'seed-user-candidate1','seed-attr-presentation','seed-opt-pres-advanced'),
                ('seed-cav-c2-english',NULL,'seed-user-candidate2','seed-attr-english','seed-opt-eng-intermediate'),
                ('seed-cav-c2-gpa','3.2','seed-user-candidate2','seed-attr-gpa',NULL),
                ('seed-cav-c2-remote','false','seed-user-candidate2','seed-attr-remote',NULL),
                ('seed-cav-c3-english',NULL,'seed-user-candidate3','seed-attr-english','seed-opt-eng-beginner');

                INSERT INTO "Cvs" ("Id","Status","Version","CreatedAt","UserId","PositionId") VALUES
                ('seed-cv-c1-ba',1,1,now(),'seed-user-candidate1','seed-pos-ba'),
                ('seed-cv-c1-jde',0,1,now(),'seed-user-candidate1','seed-pos-jde'),
                ('seed-cv-c2-jde',1,1,now(),'seed-user-candidate2','seed-pos-jde'),
                ('seed-cv-c3-ba',0,1,now(),'seed-user-candidate3','seed-pos-ba');

                INSERT INTO "DiscussionPosts" ("Id","Title","Content","CreatedAt","UserId","PositionId") VALUES
                ('seed-disc-ba-1','Looking for candidates','We are looking for candidates with strong presentation skills and advanced English for this role. Feel free to ask questions here.',now() - interval '2 days','seed-user-recruiter1','seed-pos-ba'),
                ('seed-disc-ba-2','Re: Looking for candidates','Does this role require prior consulting experience?',now() - interval '1 day','seed-user-candidate1','seed-pos-ba'),
                ('seed-disc-jde-1','Junior Data Engineer Q and A','Ask any questions about the Junior Data Engineer position here. We use Python, SQL and Spark.',now() - interval '3 days','seed-user-recruiter2','seed-pos-jde'),
                ('seed-disc-jde-2','Re: Junior Data Engineer Q and A','Is remote work available for this position?',now() - interval '2 days','seed-user-candidate2','seed-pos-jde');

                INSERT INTO "Likes" ("Id","UserId","CvId") VALUES
                ('seed-like-1','seed-user-recruiter1','seed-cv-c1-ba'),
                ('seed-like-2','seed-user-recruiter2','seed-cv-c1-ba'),
                ('seed-like-3','seed-user-recruiter1','seed-cv-c2-jde');
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DELETE FROM "Likes" WHERE "Id" LIKE 'seed-%';
                DELETE FROM "DiscussionPosts" WHERE "Id" LIKE 'seed-%';
                DELETE FROM "CandidateAttributeValues" WHERE "Id" LIKE 'seed-%';
                DELETE FROM "Cvs" WHERE "Id" LIKE 'seed-%';
                DELETE FROM "ProjectTags" WHERE "Id" LIKE 'seed-%';
                DELETE FROM "Projects" WHERE "Id" LIKE 'seed-%';
                DELETE FROM "PositionProjectTags" WHERE "Id" LIKE 'seed-%';
                DELETE FROM "PositionAttributes" WHERE "Id" LIKE 'seed-%';
                DELETE FROM "AttributeOptions" WHERE "Id" LIKE 'seed-%';
                DELETE FROM "Profiles" WHERE "Id" LIKE 'seed-%';
                DELETE FROM "Positions" WHERE "Id" LIKE 'seed-%';
                DELETE FROM "Attributes" WHERE "Id" LIKE 'seed-%';
                DELETE FROM "Users" WHERE "Id" LIKE 'seed-%';
                DELETE FROM "Tags" WHERE "Id" LIKE 'seed-%';
                DELETE FROM "Categories" WHERE "Id" LIKE 'seed-%';
                """);
        }
    }
}
