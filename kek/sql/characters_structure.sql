
/* AH */
CREATE TABLE `auctionhouse` (
  `auction_id` int(11) NOT NULL AUTO_INCREMENT,
  `auction_bid` int(11) NOT NULL,
  `auction_buyout` int(11) NOT NULL,
  `auction_timeleft` int(11) NOT NULL,
  `auction_bidder` int(11) NOT NULL DEFAULT '0',
  `auction_owner` int(11) NOT NULL,
  `auction_itemId` mediumint(11) NOT NULL,
  `auction_itemCount` tinyint(4) unsigned NOT NULL DEFAULT '1',
  `auction_itemGUID` int(11) NOT NULL,
  PRIMARY KEY (`auction_id`)
) ENGINE=MyISAM AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;


/* Corpse */
CREATE TABLE `corpse` (
  `guid` int(11) unsigned NOT NULL DEFAULT '0' COMMENT 'Global Unique Identifier',
  `player` int(11) unsigned NOT NULL DEFAULT '0' COMMENT 'Character Global Unique Identifier',
  `position_x` float NOT NULL DEFAULT '0',
  `position_y` float NOT NULL DEFAULT '0',
  `position_z` float NOT NULL DEFAULT '0',
  `orientation` float NOT NULL DEFAULT '0',
  `map` int(11) unsigned NOT NULL DEFAULT '0' COMMENT 'Map Identifier',
  `time` bigint(20) unsigned NOT NULL DEFAULT '0',
  `corpse_type` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `instance` int(11) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`guid`),
  KEY `idx_type` (`corpse_type`),
  KEY `instance` (`instance`),
  KEY `Idx_player` (`player`),
  KEY `Idx_time` (`time`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='Death System';


/* DB version */
CREATE TABLE `db_version` (
  `version` int(3) NOT NULL COMMENT 'The Version of the Release',
  `structure` int(3) NOT NULL COMMENT 'The current core structure level.',
  `content` int(3) NOT NULL COMMENT 'The current core content level.',
  `description` varchar(30) NOT NULL DEFAULT '' COMMENT 'A short description of the latest database revision.',
  `comment` varchar(150) DEFAULT '' COMMENT 'A comment about the latest database revision.',
  PRIMARY KEY (`version`,`structure`,`content`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT COMMENT='Used DB version notes';


/* Guilds */
CREATE TABLE `guilds` (
  `guild_id` int(11) NOT NULL AUTO_INCREMENT,
  `guild_name` varchar(255) NOT NULL,
  `guild_leader` int(11) NOT NULL DEFAULT '0',
  `guild_MOTD` varchar(255) NOT NULL DEFAULT '',
  `guild_info` varchar(255) NOT NULL DEFAULT '',
  `guild_cYear` int(6) unsigned NOT NULL DEFAULT '0',
  `guild_cMonth` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `guild_cDay` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `guild_tEmblemStyle` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `guild_tEmblemColor` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `guild_tBorderStyle` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `guild_tBorderColor` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `guild_tBackgroundColor` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `guild_rank0` varchar(255) NOT NULL DEFAULT 'Guild Master',
  `guild_rank0_Rights` int(11) NOT NULL DEFAULT '61951',
  `guild_rank1` varchar(255) NOT NULL DEFAULT 'Officer',
  `guild_rank1_Rights` int(11) NOT NULL DEFAULT '67',
  `guild_rank2` varchar(255) NOT NULL DEFAULT 'Veteran',
  `guild_rank2_Rights` int(11) NOT NULL DEFAULT '67',
  `guild_rank3` varchar(255) NOT NULL DEFAULT 'Member',
  `guild_rank3_Rights` int(11) NOT NULL DEFAULT '67',
  `guild_rank4` varchar(255) NOT NULL DEFAULT 'Initiate',
  `guild_rank4_Rights` int(11) NOT NULL DEFAULT '67',
  `guild_rank5` varchar(255) NOT NULL DEFAULT '',
  `guild_rank5_Rights` int(11) NOT NULL DEFAULT '0',
  `guild_rank6` varchar(255) NOT NULL DEFAULT '',
  `guild_rank6_Rights` int(11) NOT NULL DEFAULT '0',
  `guild_rank7` varchar(255) NOT NULL DEFAULT '',
  `guild_rank7_Rights` int(11) NOT NULL DEFAULT '0',
  `guild_rank8` varchar(255) NOT NULL DEFAULT '',
  `guild_rank8_Rights` int(11) NOT NULL DEFAULT '0',
  `guild_rank9` varchar(255) NOT NULL DEFAULT '',
  `guild_rank9_Rights` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`guild_id`)
) ENGINE=MyISAM AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;


/* Honor */
CREATE TABLE `characters_honor` (
  `char_guid` bigint(20) NOT NULL DEFAULT '0',
  `honor_points` smallint(6) NOT NULL DEFAULT '0',
  `honor_rank` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `honor_hightestRank` tinyint(3) NOT NULL DEFAULT '0',
  `standing_lastweek` smallint(11) NOT NULL DEFAULT '0',
  `kills_honor` mediumint(13) NOT NULL DEFAULT '0',
  `kills_dishonor` mediumint(13) NOT NULL DEFAULT '0',
  `honor_lastWeek` smallint(11) NOT NULL DEFAULT '0',
  `honor_thisWeek` smallint(11) NOT NULL DEFAULT '0',
  `honor_yesterday` smallint(11) NOT NULL DEFAULT '0',
  `kills_lastWeek` smallint(11) NOT NULL DEFAULT '0',
  `kills_thisWeek` smallint(11) NOT NULL DEFAULT '0',
  `kills_yesterday` smallint(11) NOT NULL DEFAULT '0',
  `kills_today` smallint(11) NOT NULL DEFAULT '0',
  `kills_dishonortoday` smallint(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`char_guid`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;


/* Instances */
CREATE TABLE `characters_instances` (
  `char_guid` int(8) NOT NULL,
  `map` smallint(2) unsigned NOT NULL DEFAULT '0',
  `instance` int(8) unsigned NOT NULL DEFAULT '0',
  `expire` int(8) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`char_guid`,`map`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;


/* Instances group */
CREATE TABLE `characters_instances_group` (
  `group_id` int(8) NOT NULL,
  `map` smallint(2) unsigned NOT NULL DEFAULT '0',
  `instance` int(8) unsigned NOT NULL DEFAULT '0',
  `expire` int(8) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`group_id`,`map`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;


/* Inventory */
CREATE TABLE `characters_inventory` (
  `item_guid` bigint(8) NOT NULL DEFAULT '0',
  `item_id` smallint(2) unsigned NOT NULL DEFAULT '0',
  `item_slot` tinyint(6) unsigned NOT NULL DEFAULT '255',
  `item_bag` bigint(8) NOT NULL DEFAULT '-1',
  `item_owner` bigint(8) NOT NULL DEFAULT '0',
  `item_creator` bigint(8) NOT NULL DEFAULT '0',
  `item_giftCreator` bigint(8) NOT NULL DEFAULT '0',
  `item_stackCount` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `item_durability` smallint(2) NOT NULL DEFAULT '0',
  `item_flags` smallint(11) NOT NULL DEFAULT '0',
  `item_chargesLeft` tinyint(1) NOT NULL DEFAULT '0',
  `item_textId` smallint(6) NOT NULL DEFAULT '0',
  `item_enchantment` varchar(255) NOT NULL DEFAULT '',
  `item_random_properties` smallint(6) NOT NULL DEFAULT '0',
  PRIMARY KEY (`item_guid`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;


/* Mail */
CREATE TABLE `characters_mail` (
  `mail_id` smallint(5) NOT NULL AUTO_INCREMENT,
  `mail_sender` bigint(20) NOT NULL DEFAULT '0',
  `mail_receiver` bigint(20) NOT NULL DEFAULT '0',
  `mail_type` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `mail_stationary` smallint(4) NOT NULL DEFAULT '41',
  `mail_subject` varchar(255) NOT NULL DEFAULT '',
  `mail_body` varchar(255) NOT NULL DEFAULT '',
  `mail_money` int(6) NOT NULL DEFAULT '0',
  `mail_COD` smallint(6) NOT NULL DEFAULT '0',
  `mail_time` int(6) NOT NULL DEFAULT '30',
  `mail_read` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `item_guid` bigint(20) NOT NULL,
  PRIMARY KEY (`mail_id`)
) ENGINE=MyISAM AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;


/* Pet */
CREATE TABLE `character_pet` (
  `id` int(11) unsigned NOT NULL DEFAULT '0',
  `entry` int(11) unsigned NOT NULL DEFAULT '0',
  `owner` int(11) unsigned NOT NULL DEFAULT '0',
  `modelid` int(11) unsigned DEFAULT '0',
  `CreatedBySpell` int(11) unsigned NOT NULL DEFAULT '0',
  `PetType` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `level` int(11) unsigned NOT NULL DEFAULT '1',
  `exp` int(11) unsigned NOT NULL DEFAULT '0',
  `Reactstate` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `loyaltypoints` int(11) NOT NULL DEFAULT '0',
  `loyalty` int(11) unsigned NOT NULL DEFAULT '0',
  `trainpoint` int(11) NOT NULL DEFAULT '0',
  `name` varchar(100) DEFAULT 'Pet',
  `renamed` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `slot` int(11) unsigned NOT NULL DEFAULT '0',
  `curhealth` int(11) unsigned NOT NULL DEFAULT '1',
  `curmana` int(11) unsigned NOT NULL DEFAULT '0',
  `curhappiness` int(11) unsigned NOT NULL DEFAULT '0',
  `savetime` bigint(20) unsigned NOT NULL DEFAULT '0',
  `resettalents_cost` int(11) unsigned NOT NULL DEFAULT '0',
  `resettalents_time` bigint(20) unsigned NOT NULL DEFAULT '0',
  `abdata` longtext,
  `teachspelldata` longtext,
  PRIMARY KEY (`id`),
  KEY `owner` (`owner`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='Pet System';


/* Quests */
CREATE TABLE `characters_quests` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `char_guid` bigint(20) NOT NULL DEFAULT '0',
  `quest_id` int(11) NOT NULL DEFAULT '0',
  `quest_status` int(5) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=21 DEFAULT CHARSET=utf8;


/* Social */
CREATE TABLE `character_social` (
  `guid` int(11) unsigned NOT NULL DEFAULT '0' COMMENT 'Character Global Unique Identifier',
  `friend` int(11) unsigned NOT NULL DEFAULT '0' COMMENT 'Friend Global Unique Identifier',
  `flags` tinyint(1) unsigned NOT NULL DEFAULT '0' COMMENT 'Friend Flags',
  PRIMARY KEY (`guid`,`friend`,`flags`),
  KEY `guid` (`guid`),
  KEY `friend` (`friend`),
  KEY `guid_flags` (`guid`,`flags`),
  KEY `friend_flags` (`friend`,`flags`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='Player System';


/* Spells */
CREATE TABLE `characters_spells` (
  `guid` bigint(20) unsigned NOT NULL DEFAULT '0',
  `spellid` int(8) unsigned NOT NULL DEFAULT '0',
  `active` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `cooldown` int(8) unsigned NOT NULL DEFAULT '0',
  `cooldownitem` int(8) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`guid`,`spellid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


/* Tickets */
CREATE TABLE `characters_tickets` (
  `char_guid` bigint(20) NOT NULL DEFAULT '0',
  `ticket_text` text NOT NULL,
  `ticket_x` float NOT NULL DEFAULT '0',
  `ticket_y` float NOT NULL DEFAULT '0',
  `ticket_z` float NOT NULL DEFAULT '0',
  `ticket_map` int(8) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`char_guid`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;


/* Characters */
CREATE TABLE `characters` (
  `account_id` mediumint(3) unsigned NOT NULL DEFAULT '0',
  `char_guid` int(8) NOT NULL AUTO_INCREMENT,
  `char_name` varchar(21) NOT NULL DEFAULT '',
  `char_level` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `char_xp` mediumint(3) NOT NULL DEFAULT '0',
  `char_xp_rested` mediumint(3) NOT NULL DEFAULT '0',
  `char_online` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `char_logouttime` int(8) unsigned NOT NULL DEFAULT '0',
  `char_positionX` float NOT NULL DEFAULT '0',
  `char_positionY` float NOT NULL DEFAULT '0',
  `char_positionZ` float NOT NULL DEFAULT '0',
  `char_map_id` smallint(2) NOT NULL DEFAULT '0',
  `char_zone_id` smallint(2) NOT NULL DEFAULT '0',
  `char_orientation` float NOT NULL DEFAULT '0',
  `char_moviePlayed` tinyint(1) NOT NULL DEFAULT '0',
  `bindpoint_positionX` float NOT NULL DEFAULT '0',
  `bindpoint_positionY` float NOT NULL DEFAULT '0',
  `bindpoint_positionZ` float NOT NULL DEFAULT '0',
  `bindpoint_map_id` smallint(2) NOT NULL DEFAULT '0',
  `bindpoint_zone_id` smallint(2) NOT NULL DEFAULT '0',
  `char_guildId` int(1) NOT NULL DEFAULT '0',
  `char_guildRank` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `char_guildPNote` varchar(255) NOT NULL DEFAULT '',
  `char_guildOffNote` varchar(255) NOT NULL DEFAULT '',
  `char_race` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `char_class` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `char_gender` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `char_skin` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `char_face` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `char_hairStyle` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `char_hairColor` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `char_facialHair` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `char_restState` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `char_mana` smallint(2) NOT NULL DEFAULT '1',
  `char_energy` smallint(2) NOT NULL DEFAULT '0',
  `char_rage` smallint(2) NOT NULL DEFAULT '0',
  `char_life` smallint(2) NOT NULL DEFAULT '1',
  `char_manaType` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `char_strength` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `char_agility` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `char_stamina` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `char_intellect` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `char_spirit` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `char_copper` int(6) unsigned NOT NULL DEFAULT '0',
  `char_watchedFactionIndex` tinyint(1) unsigned NOT NULL DEFAULT '255',
  `char_reputation` text NOT NULL,
  `char_skillList` text NOT NULL,
  `char_auraList` text NOT NULL,
  `char_tutorialFlags` varchar(255) NOT NULL DEFAULT '',
  `char_taxiFlags` varchar(255) NOT NULL DEFAULT '',
  `char_actionBar` text NOT NULL,
  `char_mapExplored` text NOT NULL,
  `force_restrictions` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `char_talentPoints` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `char_bankSlots` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `char_transportGuid` bigint(8) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`char_guid`)
) ENGINE=MyISAM AUTO_INCREMENT=48 DEFAULT CHARSET=utf8;


/* Petitions */
CREATE TABLE IF NOT EXISTS `petitions` (
  `petition_id` int(11) NOT NULL,
  `petition_itemGuid` int(11) NOT NULL,
  `petition_owner` int(11) NOT NULL,
  `petition_name` varchar(255) NOT NULL,
  `petition_type` tinyint(3) unsigned NOT NULL,
  `petition_signedMembers` tinyint(3) unsigned NOT NULL,
  `petition_signedMember1` int(11) NOT NULL DEFAULT '0',
  `petition_signedMember2` int(11) NOT NULL DEFAULT '0',
  `petition_signedMember3` int(11) NOT NULL DEFAULT '0',
  `petition_signedMember4` int(11) NOT NULL DEFAULT '0',
  `petition_signedMember5` int(11) NOT NULL DEFAULT '0',
  `petition_signedMember6` int(11) NOT NULL DEFAULT '0',
  `petition_signedMember7` int(11) NOT NULL DEFAULT '0',
  `petition_signedMember8` int(11) NOT NULL DEFAULT '0',
  `petition_signedMember9` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`petition_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;




