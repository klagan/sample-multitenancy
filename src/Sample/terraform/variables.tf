locals {

  wc_homepage  = var.webclient_baseaddress != "" ? var.webclient_baseaddress : "https://not-defined.com"
  wa1_homepage = var.webapi1_baseaddress != "" ? var.webapi1_baseaddress : "https://not-defined.com"
  wa2_homepage = var.webapi2_baseaddress != "" ? var.webapi2_baseaddress : "https://not-defined.com"
}

variable webclient_user_secret_id {}

variable webclient_replyurl {}

variable webclient_name {}

variable webapi1_user_secret_id {}

variable webapi1_replyurl {}

variable webapi1_name {}

variable webapi2_user_secret_id {}

variable webapi2_replyurl {}

variable webapi2_name {}

variable webclient_baseaddress {}

variable webapi1_baseaddress {}

variable webapi2_baseaddress {}