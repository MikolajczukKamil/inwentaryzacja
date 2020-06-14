<?php


class ScanPosition implements JsonSerializable
{
    private $asset;
    private $state;

    /**
     * @return Asset
     */
    public function getAsset()
    {
        return $this->asset;
    }

    /**
     * @param Asset $asset
     */
    public function setAsset(Asset $asset): void
    {
        $this->asset = $asset;
    }

    /**
     * @return integer
     */
    public function getState()
    {
        return $this->state;
    }

    /**
     * @param integer $state
     */
    public function setState(int $state): void
    {
        $this->state = $state;
    }

    /**
     * @inheritDoc
     */
    public function jsonSerialize()
    {
        return [
            'asset' => $this->asset,
            'state' => $this->state
        ];
    }
}