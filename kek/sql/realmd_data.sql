
/* Current DB version */
INSERT INTO `db_version`(`version`,`structure`,`content`,`description`,`comment`) VALUES 
(21,2,1,'Add_field_comments','Base Database from 20150409 to Rel21_2_1');


/* Default accounts, password is the same as username */
INSERT INTO `account` (`username`, `sha_pass_hash`, `gmlevel`) VALUES 
('ADMINISTRATOR','a34b29541b87b7e4823683ce6c7bf6ae68beaaac',3),
('GAMEMASTER','7841e21831d7c6bc0b57fbe7151eb82bd65ea1f9',2),
('MODERATOR','a7f5fbff0b4eec2d6b6e78e38e8312e64d700008',1),
('PLAYER','3ce8a96d17c5ae88a30681024e86279f1a38c041',0);


/* Default realm */
INSERT INTO `realmlist` (`name`, `realmflags`) VALUES ('MangosSharp Test Server', 32);

