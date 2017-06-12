//
//  DailyTvCell.swift
//  XWeather
//
//  Created by Colby Williams on 6/11/17.
//  Copyright © 2017 Colby Williams. All rights reserved.
//

import UIKit

class DailyTvCell: UITableViewCell {
	
	@IBOutlet var dayLabel: UILabel!
	@IBOutlet var highTempLabel: UILabel!
	@IBOutlet var iconImageView: UIImageView!
	@IBOutlet var lowTempLabel: UILabel!
	@IBOutlet var precipLabel: UILabel!
	
    override func awakeFromNib() {
        super.awakeFromNib()
        // Initialization code
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }

}
