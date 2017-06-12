//
//  LocationTvCell.swift
//  XWeather
//
//  Created by Colby Williams on 6/11/17.
//  Copyright © 2017 Colby Williams. All rights reserved.
//

import UIKit

class LocationTvCell: UITableViewCell {
	
	@IBOutlet var currentImage: UIImageView!
	@IBOutlet var nameLabel: UILabel!
	@IBOutlet var tempLabel: UILabel!
	@IBOutlet var timeLabel: UILabel!
	
    override func awakeFromNib() {
        super.awakeFromNib()
        // Initialization code
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }

}
